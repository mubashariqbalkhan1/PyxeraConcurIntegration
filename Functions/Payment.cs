using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace PyxeraConcurIntegrationConsole
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly CommonFunctions _commonFunctions;

        public PaymentService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _commonFunctions = new CommonFunctions(httpClient, config);
            _config = config;
        }
        public async Task<List<Job>> FetchPaymentJobsAsync()
        {
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();

            string Payment_Types = _config["Concur:Payment_Types"];
            var reports = new List<Definition>();

            var jsonTypes = await _commonFunctions.GetConcurDataAsync(accessToken, Payment_Types);
            var serializedTypes = JsonConvert.DeserializeObject<RootDefinition>(jsonTypes);

            reports.AddRange(serializedTypes.definitions.definition);
            List<Job> jobs = new List<Job>();

            List<string> paymentsRequired = new List<string> { "BREX Mastercard - 20220331080753", "USD Synchronized Accounting Extract", "US/USD Cash Advance Expense Pay By Concur Synchronized Accounting Extract" };
            List<Definition> paymentsToSync = reports.Where(pt => paymentsRequired.Contains(pt.name)).ToList();
            foreach (var item in paymentsToSync)
            {
                string Payment_Jobs = _config["Concur:Payment_Jobs"];
                var jobid = item.joblink.Split('/')[7];
                Payment_Jobs = Payment_Jobs.Replace("{jobid}", jobid);
                var jsonJobs = await _commonFunctions.GetConcurDataAsync(accessToken, Payment_Jobs);
                var serializedJobs = JsonConvert.DeserializeObject<RootJob>(jsonJobs);
                var recentJob = serializedJobs.jobs.job.OrderByDescending(j => j.starttime).FirstOrDefault();
                if (recentJob != null)
                {
                    recentJob.jobname = item.name;
                    recentJob.jobid = jobid;
                    jobs.Add(recentJob);
                }
            }
            return jobs;
        }

        #region
        public async Task<List<BrexMasterCard>> FetchBrexMasterCards(string url)
        {
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            var csvData = await _commonFunctions.GetConcurStringDataAsync(accessToken, url);

            var expenses = new List<BrexMasterCard>();
            if (string.IsNullOrWhiteSpace(csvData)) return expenses;

            // crude delimiter detection (header line)
            var headerLine = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            var delimiter = (headerLine != null && headerLine.Contains('\t')) ? "\t" : ",";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = delimiter,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null,        // ignore bad data callback
                MissingFieldFound = null    // ignore missing field callback
            };

            using var reader = new StringReader(csvData);
            using var csv = new CsvReader(reader, config);

            // read header
            if (!csv.Read()) return expenses;
            csv.ReadHeader();

            while (csv.Read())
            {
                var record = csv.Context.Parser.Record; // string[] of fields for current row

                string F(int i) => i < record.Length ? record[i] : string.Empty;
                try
                {
                    var entry = new BrexMasterCard();
                    entry.DATE = ParseDate(F(0)) ?? DateTime.MinValue;
                    entry.EmployeeID = F(1);
                    entry.EmployeeLastName = F(2);
                    entry.EmployeeFirstName = F(3);
                    entry.ReportID = F(4);
                    entry.EmployeeDefaultCurrencyAlphaCode = F(5);
                    entry.ReportSubmitDate = !string.IsNullOrWhiteSpace(F(6)) ? ParseDate(F(6)) : null;
                    entry.ReportProcessingPaymentDate = !string.IsNullOrWhiteSpace(F(7)) ? ParseDate(F(7)) : null;
                    entry.ReportName = F(8);
                    entry.ReportEntryExpenseTypeName = F(9);
                    entry.ReportEntryTransactionDate = !string.IsNullOrWhiteSpace(F(10)) ? ParseDate(F(10)) : null;
                    entry.ReportEntryIsPersonalFlag = F(11);
                    entry.ReportEntryDescription = F(12);
                    entry.ReportEntryVendorName = F(13);
                    entry.ReportEntryVendorDescription = F(14);
                    entry.ReportEntryPaymentCodeCode = F(15);
                    entry.ReportEntryPaymentCodeName = F(16);
                    entry.ReportPaymentReimbursementType = F(17);
                    entry.BilledCreditCardAccountNumber = F(18);
                    entry.BilledCreditCardAccountDescription = F(19);
                    entry.JournalPayerPaymentCodeName = F(20);
                    entry.JournalPayeePaymentTypeName = F(21);

                    if (!string.IsNullOrWhiteSpace(F(22)) &&
                        decimal.TryParse(F(22), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                        entry.JournalAmount = amount;
                    else
                        entry.JournalAmount = 0;

                    entry.JournalAccountCode = F(23);
                    entry.JournalDebitOrCredit = F(24);
                    entry.PaymentDemandCompanyCashAccountCode = F(25);
                    entry.PaymentDemandCompanyLiabilityAccountCode = F(26);
                    entry.EstimatedPaymentDate = !string.IsNullOrWhiteSpace(F(27)) ? ParseDate(F(27)) : null;
                    entry.Program = F(28);
                    entry.Department = F(29);
                    entry.AllocationCustom6 = F(30);
                    entry.Activity = F(31);
                    entry.AdditionalExplanation = F(32);

                    expenses.Add(entry);
                }
                catch (Exception ex)
                {
                    // log then continue
                    Console.WriteLine($"Error parsing record: {ex.Message}");
                }
            }

            return expenses;
        }
        public async Task<List<BrexMasterCard>> FetchExpensePay(string url)
        {
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            var csvData = await _commonFunctions.GetConcurStringDataAsync(accessToken, url);

            var expenses = new List<BrexMasterCard>();
            var lines = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(1))
            {
                var delimiter = line.Contains('\t') ? '\t' : ',';
                var record = line.Split(delimiter);

                string F(int i) => i < record.Length ? record[i] : string.Empty;

                try
                {
                    var entry = new BrexMasterCard
                    {
                        DATE = ParseDate(F(0)) ?? DateTime.MinValue,
                        EmployeeID = F(1),
                        EmployeeLastName = F(2),
                        EmployeeFirstName = F(3),
                        ReportID = F(4),
                        EmployeeDefaultCurrencyAlphaCode = F(5),
                        ReportSubmitDate = !string.IsNullOrWhiteSpace(F(6)) ? ParseDate(F(6)) : null,
                        ReportProcessingPaymentDate = !string.IsNullOrWhiteSpace(F(7)) ? ParseDate(F(7)) : null,
                        ReportName = F(8),
                        ReportEntryExpenseTypeName = F(9),
                        ReportEntryTransactionDate = !string.IsNullOrWhiteSpace(F(10)) ? ParseDate(F(10)) : null,
                        ReportEntryIsPersonalFlag = F(11),
                        ReportEntryDescription = F(12),
                        ReportEntryVendorName = F(13),
                        ReportEntryVendorDescription = F(14),
                        ReportEntryPaymentCodeCode = F(15),
                        ReportEntryPaymentCodeName = F(16),
                        ReportPaymentReimbursementType = F(17),
                        BilledCreditCardAccountNumber = F(18),
                        BilledCreditCardAccountDescription = F(19),
                        JournalPayerPaymentCodeName = F(20),
                        JournalPayeePaymentTypeName = F(21),
                        JournalAmount = !string.IsNullOrWhiteSpace(F(22)) && decimal.TryParse(F(22), out var amount) ? amount : 0,
                        JournalAccountCode = F(23),
                        JournalDebitOrCredit = F(24),
                        PaymentDemandCompanyCashAccountCode = F(25),
                        PaymentDemandCompanyLiabilityAccountCode = F(26),
                        EstimatedPaymentDate = !string.IsNullOrWhiteSpace(F(27)) ? ParseDate(F(27)) : null,
                        Program = F(28),
                        Department = F(29),
                        AllocationCustom6 = F(30),
                        Activity = F(31),
                        AdditionalExplanation = F(32)
                    };

                    expenses.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(line);
                    Console.WriteLine($"Error parsing line: {ex.Message}");
                }
            }

            return expenses;
        }
        public async Task<List<BrexMasterCard>> FetchCompanyCheck(string url)
        {
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            var csvData = await _commonFunctions.GetConcurStringDataAsync(accessToken, url);

            var expenses = new List<BrexMasterCard>();
            var lines = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(1))
            {
                var delimiter = line.Contains('\t') ? '\t' : ',';
                var record = line.Split(delimiter);

                string F(int i) => i < record.Length ? record[i] : string.Empty;

                try
                {
                    var entry = new BrexMasterCard
                    {
                        DATE = ParseDate(F(0)) ?? DateTime.MinValue,
                        EmployeeID = F(1),
                        EmployeeLastName = F(2),
                        EmployeeFirstName = F(3),
                        ReportID = F(4),
                        EmployeeDefaultCurrencyAlphaCode = F(5),
                        ReportSubmitDate = !string.IsNullOrWhiteSpace(F(6)) ? ParseDate(F(6)) : null,
                        ReportProcessingPaymentDate = !string.IsNullOrWhiteSpace(F(7)) ? ParseDate(F(7)) : null,
                        ReportName = F(8),
                        ReportEntryExpenseTypeName = F(9),
                        ReportEntryTransactionDate = !string.IsNullOrWhiteSpace(F(10)) ? ParseDate(F(10)) : null,
                        ReportEntryIsPersonalFlag = F(11),
                        ReportEntryDescription = F(12),
                        ReportEntryVendorName = F(13),
                        ReportEntryVendorDescription = F(14),
                        ReportEntryPaymentCodeCode = F(15),
                        ReportEntryPaymentCodeName = F(16),
                        ReportPaymentReimbursementType = F(17),
                        BilledCreditCardAccountNumber = F(18),
                        BilledCreditCardAccountDescription = F(19),
                        JournalPayerPaymentCodeName = F(20),
                        JournalPayeePaymentTypeName = F(21),
                        JournalAmount = !string.IsNullOrWhiteSpace(F(22)) && decimal.TryParse(F(22), out var amount) ? amount : 0,
                        JournalAccountCode = F(23),
                        JournalDebitOrCredit = F(24),
                        PaymentDemandCompanyCashAccountCode = F(25),
                        PaymentDemandCompanyLiabilityAccountCode = F(26),
                        EstimatedPaymentDate = !string.IsNullOrWhiteSpace(F(27)) ? ParseDate(F(27)) : null,
                        Program = F(28),
                        Department = F(29),
                        AllocationCustom6 = F(30),
                        Activity = F(31),
                        AdditionalExplanation = F(32)
                    };

                    expenses.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(line);
                    Console.WriteLine($"Error parsing line: {ex.Message}");
                }
            }

            return expenses;
        }
        public async Task<List<CashAdvance>> FetchCashAdvance(string url)
        {
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            var csvData = await _commonFunctions.GetConcurStringDataAsync(accessToken, url);

            var expenses = new List<CashAdvance>();
            var lines = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(1))
            {
                var delimiter = line.Contains('\t') ? '\t' : ',';
                var record = line.Split(delimiter);

                string F(int i) => i < record.Length ? record[i] : string.Empty;

                try
                {
                    var entry = new CashAdvance
                    {
                        CashAdvanceRequestAmount = !string.IsNullOrWhiteSpace(F(0)) ? decimal.Parse(F(0)) : 0m,
                        CashAdvanceRequestCurrencyAlphaCode = F(1),
                        CashAdvanceRequestCurrencyNumericCode = !string.IsNullOrWhiteSpace(F(2)) ? int.Parse(F(2)) : 0,
                        CashAdvanceExchangeRate = !string.IsNullOrWhiteSpace(F(3)) ? decimal.Parse(F(3)) : 0m,
                        CashAdvanceCurrencyAlphaCode = F(4),
                        CashAdvanceCurrencyNumericCode = !string.IsNullOrWhiteSpace(F(5)) ? int.Parse(F(5)) : 0,
                        CashAdvanceIssuedDate = !string.IsNullOrWhiteSpace(F(6)) ? ParseDate(F(6)) : DateTime.MinValue,
                        CashAdvancePaymentCodeName = F(7),
                        CashAdvanceTransactionType = !string.IsNullOrWhiteSpace(F(8)) ? int.Parse(F(8)) : 0,
                        CashAdvanceRequestDate = !string.IsNullOrWhiteSpace(F(9)) ? ParseDate(F(9)) : DateTime.MinValue,
                        CashAdvanceKey = !string.IsNullOrWhiteSpace(F(10)) ? int.Parse(F(10)) : 0,
                        CashAdvancePaymentMethod = !string.IsNullOrWhiteSpace(F(11)) ? int.Parse(F(11)) : 0,
                        EmployeeID = F(12),
                        EmployeeLastName = F(13),
                        EmployeeFirstName = F(14),
                        JournalPayerPaymentTypeName = F(15),
                        JournalPayerPaymentCodeName = F(16),
                        JournalPayeePaymentTypeName = F(17),
                        JournalPayeePaymentCodeName = F(18),
                        JournalAccountCode = F(19),
                        JournalDebitOrCredit = F(20),
                        JournalAmount = !string.IsNullOrWhiteSpace(F(21)) ? decimal.Parse(F(21)) : 0m,
                        BatchID = !string.IsNullOrWhiteSpace(F(22)) ? int.Parse(F(22)) : 0,
                        BatchDate = !string.IsNullOrWhiteSpace(F(23)) ? ParseDate(F(23)) : DateTime.MinValue
                    };

                    expenses.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(line);
                    Console.WriteLine($"Error parsing line: {ex.Message}");
                }
            }

            return expenses;
        }
        #endregion

        private DateTime? ParseDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParseExact(value,
                new[] { "MM/dd/yy", "MM/dd/yyyy" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result))
            {
                return result;
            }

            return null; // fallback if invalid
        }

        public async Task SendToBC_BrexMaster(List<BrexMasterCard> brexMasterCards)
        {
            bool hasData = true;
            var list = new List<PostBrexMaster>();
            var requestUrl = _config["BusinessCentral:BC_BrexMaster_API"];
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC Brex Master Card data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<PostBrexMaster>>(data);
                if (!string.IsNullOrEmpty(parsedBcData.odatanextLink))
                {
                    requestUrl = parsedBcData.odatanextLink;
                }
                else
                {
                    hasData = false;
                }
                list.AddRange(parsedBcData.value);
            } while (hasData);
            var linesToSend = new List<PostBrexMaster>();
            foreach (var item in brexMasterCards)
            {
                try
                {
                    linesToSend.Add(new PostBrexMaster(item));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing Brex Master Card: {ex.Message}");
                }
            }
            await _commonFunctions.SendDataToBc(linesToSend, requestUrl, bcToken, "BrexMaster");
        }
        public async Task SendToBC_CompanyCheck(List<BrexMasterCard> brexMasterCards)
        {
            bool hasData = true;
            var list = new List<PostBrexMaster>();
            var requestUrl = _config["BusinessCentral:BC_CompanyCheck_API"];
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC Brex Master Card data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<PostBrexMaster>>(data);
                if (!string.IsNullOrEmpty(parsedBcData.odatanextLink))
                {
                    requestUrl = parsedBcData.odatanextLink;
                }
                else
                {
                    hasData = false;
                }
                list.AddRange(parsedBcData.value);
            } while (hasData);
            var linesToSend = new List<PostCompanyCheck>();
            foreach (var item in brexMasterCards)
            {
                try
                {
                //  if (!list.Any(bc => bc.ReportId.ToLower() == item.ReportID.ToLower()))
                    linesToSend.Add(new PostCompanyCheck(item));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing Company Check: {ex.Message}");
                }
            }
            await _commonFunctions.SendDataToBc(linesToSend, requestUrl, bcToken, "CompanyCheck");
        }
        public async Task SendToBC_ExpensePay(List<BrexMasterCard> brexMasterCards)
        {
            bool hasData = true;
            var list = new List<PostBrexMaster>();
            var requestUrl = _config["BusinessCentral:BC_ExpensePay_API"];
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC Expense Pay data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<PostBrexMaster>>(data);
                if (!string.IsNullOrEmpty(parsedBcData.odatanextLink))
                {
                    requestUrl = parsedBcData.odatanextLink;
                }
                else
                {
                    hasData = false;
                }
                list.AddRange(parsedBcData.value);
            } while (hasData);
            var linesToSend = new List<PostExpensePay>();
            foreach (var item in brexMasterCards)
            {
                try
                {
                    //if (!list.Any(bc => bc.ReportId.ToLower() == item.ReportID.ToLower()))
                    linesToSend.Add(new PostExpensePay(item));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing Expense Pay: {ex.Message}");
                }
            }
            await _commonFunctions.SendDataToBc(linesToSend, requestUrl, bcToken, "ExpensePay");
        }
        public async Task SendToBC_CashAdvance(List<CashAdvance> cashAdvances)
        {
            bool hasData = true;
            var list = new List<PostCashAdvance>();
            var requestUrl = _config["BusinessCentral:BC_CashAdvance_API"];
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC Cash Advance data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<PostCashAdvance>>(data);
                if (!string.IsNullOrEmpty(parsedBcData.odatanextLink))
                {
                    requestUrl = parsedBcData.odatanextLink;
                }
                else
                {
                    hasData = false;
                }
                list.AddRange(parsedBcData.value);
            } while (hasData);
            var linesToSend = new List<PostCashAdvance>();
            foreach (var item in cashAdvances)
            {
                try
                {
                    // if (!list.Any(bc => bc.CashAdvanceKey == item.CashAdvanceKey))
                    // {
                    linesToSend.Add(new PostCashAdvance(item));
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing Cash Advance: {ex.Message}");
                }
            }
            await _commonFunctions.SendDataToBc(linesToSend, requestUrl, bcToken, "CashAdvance");
        }
    }
}