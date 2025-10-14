using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PyxeraConcurIntegrationConsole
{
    public class ConcurSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly CommonFunctions _commonFunctions;
        private readonly ExpenseService _expenseService;
        private readonly InvoiceService _invoiceService;
        private readonly PaymentService _paymentService;

        public ConcurSyncService(HttpClient httpClient, ILogger<ConcurSyncService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
            _commonFunctions = new CommonFunctions(httpClient, config);
            _expenseService = new ExpenseService(httpClient, config);
            _invoiceService = new InvoiceService(httpClient, config);
            _paymentService = new PaymentService(httpClient, config);
        }

        public async Task RunAsync()
        {
            var date = DateTime.Now;
            _logger.LogInformation($"Starting Concur → BC sync at: {date}");

            try
            {
                // ----- Fetch Headers -----
                List<Report> reports = await _expenseService.FetchHeaders();
                await _expenseService.SendToBc_ExpensesHeader(reports);

                // ----- Fetch Expense Allocation -----
                List<ReportAllocation> expenseAllocations = await _expenseService.FetchExpenseAllocations();
                await _expenseService.SendToBc_ExpensesHeaderAllocations(expenseAllocations);

                // -- Fetch Expense Cash Advances ---
                List<ExpenseCashAdvance> cashAdvances = await _expenseService.FetchExpenseCashAdvance(reports);
                //distinct cashAdvances
                var cashAdvances1 = cashAdvances
                    .GroupBy(ca => ca.CashAdvanceId)
                    .Select(g => g.First())
                    .ToList();

                var duplicates = cashAdvances
                    .GroupBy(ca => ca.CashAdvanceId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                await _expenseService.SendToBc_ExpensesHeaderCashAdvance(cashAdvances1);

                // ----- Fetch Entries -----
                List<Entry> entries = await _expenseService.FetchEntries();
                await _expenseService.SendToBc_ExpensesHeaderEntries(entries);

                // // ----- Fetch Itemizations -----
                List<Itemization> itemizations = await _expenseService.FetchItemizations();
                await _expenseService.SendToBc_ExpensesHeaderItemization(itemizations);

                // // ----- Fetch Invoice Digests -----
                List<PaymentRequest> invoiceDigests = await _invoiceService.FetchInvoiceDigest();
                await _invoiceService.SendToBc_InvoiceHeaders(invoiceDigests);
                await _invoiceService.SendToBc_InvoiceLines(invoiceDigests);
                await _invoiceService.SendToBc_InvoiceLineAllocations(invoiceDigests);

                // ----- Fetch Payment Jobs -----
                var stateManager = new StateManager("state.json");
                var state = stateManager.Load();
                string currentMonth = DateTime.UtcNow.ToString("yyyy-MM");

                Console.WriteLine("******************************************************************");
                Console.WriteLine($"▶ Processing jobs for {currentMonth}...");
                Console.WriteLine(JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true }));
                Console.WriteLine("******************************************************************");

                List<Job> paymentJobs = await _paymentService.FetchPaymentJobsAsync();

                // Only keep jobs from current month
                var paymentJobsToRun = paymentJobs
                    .Where(j => j.starttime.ToString("yyyy-MM") == currentMonth)
                    .OrderByDescending(j => j.starttime)
                    .ToList();

                if (!paymentJobsToRun.Any())
                {
                    Console.WriteLine($"No payment jobs found for {currentMonth}, skipping.");
                }
                else
                {
                    foreach (var item in paymentJobsToRun)
                    {
                        bool alreadyProcessed = state.ProcessedJobs
                        .Any(j => j.JobId == item.jobid && j.Month == currentMonth);

                        if (alreadyProcessed)
                        {
                            Console.WriteLine($"Job {item.jobid} already processed for {currentMonth}, skipping.");
                            continue;
                        }
                        Console.WriteLine($"Processing job: {item.jobname} (ID: {item.jobid})");

                        //Comma Separated
                        //BREX Mastercard - 20220331080753
                        if (item.jobid == "gWuuTW0yZqyrUxfH3PimnoigozZ2uyOYxZQ")
                        {
                            var brexmaster = await _paymentService.FetchBrexMasterCards(item.filelink);
                            await _paymentService.SendToBC_BrexMaster(brexmaster);
                        }
                        //Tab Separated
                        //Have same data as Brex
                        //Company Pay
                        //USD Synchronized Accounting Extract
                        else if (item.jobid == "gWuuTW0ycpyavfs6tj6lXrKS3xmx7DUC1xg")
                        {
                            var usdsync = await _paymentService.FetchCompanyCheck(item.filelink);
                            await _paymentService.SendToBC_CompanyCheck(usdsync);
                        }
                        //Tab Separated
                        //Have same data as Brex
                        //Expense Pay
                        //USD Synchronized Accounting Extract
                        else if (item.jobid == "gWuuTW0ye31DVip7hmZNZbEx6hbirkC8S3A")
                        {
                            var usdsync = await _paymentService.FetchExpensePay(item.filelink);
                            await _paymentService.SendToBC_ExpensePay(usdsync);
                        }
                        //Comma Separated
                        //Have different data
                        //Cash Advances
                        //US/USD Cash Advance Expense Pay By Concur Synchronized Accounting Extract
                        else if (item.jobid == "gWuuTW0ye3SekV7dbEKfwOFbGkWYm4ARhDg")
                        {
                            var cashadv = await _paymentService.FetchCashAdvance(item.filelink);
                            await _paymentService.SendToBC_CashAdvance(cashadv);
                        }
                        else
                        {
                            _logger.LogInformation($"No handler for job name: {item.jobname}");
                            continue;
                        }
                        // ---- Mark as processed ----
                        //state.ProcessedJobs.RemoveAll(j => j.JobId == item.jobid);
                        state.ProcessedJobs.Add(new ProcessedJob
                        {
                            JobId = item.jobid,
                            Month = currentMonth
                        });
                        stateManager.Save(state);
                    }

                    // Save state after all jobs processed
                    state.LastRunDate = DateTime.UtcNow;
                    stateManager.Save(state);
                    Console.WriteLine($"Jobs for {currentMonth} processed and state updated.");
                }

                var endDate = DateTime.Now;
                var duration = endDate - date;
                _logger.LogInformation($"Synchronization completed successfully in {duration.Hours} hours {duration.Minutes} minutes and {duration.Seconds} seconds!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
            }
        }

    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Detect environment (default = Production)
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "production";
            //environment = "development"; // Forcing to development for testing

            Console.WriteLine($"Environment: {environment}");

            // Build DI container
            var services = new ServiceCollection();

            // Load config (base + environment-specific)
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine($"Loaded configuration sources:");
            foreach (var p in ((IConfigurationRoot)config).Providers)
            {
                Console.WriteLine($" - {p}");
            }

            services.AddSingleton<IConfiguration>(config);
            services.AddLogging(builder =>
            {
                builder.AddConsole();

                // Suppress internal .NET and HTTP client logs
                builder.AddFilter("System.Net.Http.HttpClient", LogLevel.None);
                builder.AddFilter("System", LogLevel.Warning);
                builder.AddFilter("Microsoft", LogLevel.Warning);
            });

            services.AddHttpClient<ConcurSyncService>(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(5); // Increase timeout to 5 minutes
            });

            var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<ConcurSyncService>();

            await service.RunAsync();
        }
    }

}
