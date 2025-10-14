using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PyxeraConcurIntegrationConsole
{
    public class ExpenseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly CommonFunctions _commonFunctions;

        public ExpenseService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _commonFunctions = new CommonFunctions(httpClient, config);
            _config = config;
        }

        public async Task<List<Report>> FetchHeaders()
        {
            Console.WriteLine("Fetching Expense Headers from Concur...");
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            string url = _config["Concur:Expense_Header"];
            var reports = new List<Report>();
            bool hasData = true;
            var concurTokenTime = DateTime.Now;

            while (hasData)
            {
                // Refresh Concur token every 25 minutes
                if ((DateTime.Now - concurTokenTime).TotalMinutes > 25)
                {
                    accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
                    concurTokenTime = DateTime.Now;
                }
                var json = await _commonFunctions.GetConcurDataAsync(accessToken, url);
                var serialized = JsonConvert.DeserializeObject<ExpensesHeaderReport>(json);

                if (string.IsNullOrEmpty(serialized.Reports.NextPage))
                    hasData = false;
                else
                {
                    url = serialized.Reports.NextPage;
                }
                reports.AddRange(serialized.Reports.Items.Report);
            }
            return reports;
        }
        public async Task<List<ReportAllocation>> FetchExpenseAllocations()
        {
            Console.WriteLine("Fetching Expense Allocations from Concur...");
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            string url = _config["Concur:Expense_Allocations"];
            var reports = new List<ReportAllocation>();
            bool hasData = true;
            var concurTokenTime = DateTime.Now;

            while (hasData)
            {
                // Refresh Concur token every 25 minutes
                if ((DateTime.Now - concurTokenTime).TotalMinutes > 25)
                {
                    accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
                    concurTokenTime = DateTime.Now;
                }
                var json = await _commonFunctions.GetConcurDataAsync(accessToken, url);
                var serialized = JsonConvert.DeserializeObject<ExpensesHeaderReportAllocation>(json);

                if (string.IsNullOrEmpty(serialized.Allocations.NextPage))
                    hasData = false;
                else
                {
                    url = serialized.Allocations.NextPage;
                }
                reports.AddRange(serialized.Allocations.Items.Allocation);
            }
            return reports;
        }

        public async Task<List<ExpenseCashAdvance>> FetchExpenseCashAdvance(List<Report> report)
        {
            Console.WriteLine("Fetching Expense Cash Advances from Concur...");
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            string url = _config["Concur:Expense_CashAdvId"];
            var reports = new List<ExpenseCashAdvance>();
            int total = report.Count;
            int count = 0;

            foreach (var item in report)
            {
                string requestUrl = url.Replace("{id}", item.ID);
                var json = await _commonFunctions.GetConcurDataJsonAsync(accessToken, requestUrl);
                var serialized = JsonConvert.DeserializeObject<CashAdvanceResponse>(json);
                if (serialized.CashAdvances != null && serialized.CashAdvances.Count > 0)
                {
                    foreach (var item1 in serialized.CashAdvances)
                    {
                        if (!string.IsNullOrEmpty(item1.Id))
                        {
                            string url1 = _config["Concur:Expense_CashAdvance"];
                            string requestUrl1 = url1.Replace("{id}", item1.Id);
                            var json1 = await _commonFunctions.GetConcurDataJsonAsync(accessToken, requestUrl1);
                            var serialized1 = JsonConvert.DeserializeObject<ExpenseCashAdvance>(json1);
                            if (serialized1 != null)
                            {
                                serialized1.ReportId = item.ID;
                                reports.Add(serialized1);
                            }
                        }
                    }
                }
                count++;
            }
            return reports;
        }

        public async Task<List<Entry>> FetchEntries()
        {
            Console.WriteLine("Fetching Expense Entries from Concur...");
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            string url = _config["Concur:Expense_Entries"];
            var entries = new List<Entry>();
            bool hasData = true;
            var concurTokenTime = DateTime.Now;

            while (hasData)
            {
                // Refresh Concur token every 25 minutes
                if ((DateTime.Now - concurTokenTime).TotalMinutes > 25)
                {
                    accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
                    concurTokenTime = DateTime.Now;
                }
                var json = await _commonFunctions.GetConcurDataAsync(accessToken, url);
                var serialized = JsonConvert.DeserializeObject<ExpensesHeaderEntries>(json);

                if (string.IsNullOrEmpty(serialized.Entries.NextPage))
                    hasData = false;
                else
                {
                    url = serialized.Entries.NextPage;
                }
                entries.AddRange(serialized.Entries.Items.Entry);
            }
            return entries;
        }

        public async Task<List<Itemization>> FetchItemizations()
        {
            Console.WriteLine("Fetching Expense Itemizations from Concur...");
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            string url = _config["Concur:Expense_Itemizations"];
            var items = new List<Itemization>();
            bool hasData = true;
            var concurTokenTime = DateTime.Now;

            while (hasData)
            {
                // Refresh Concur token every 25 minutes
                if ((DateTime.Now - concurTokenTime).TotalMinutes > 25)
                {
                    accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
                    concurTokenTime = DateTime.Now;
                }
                var json = await _commonFunctions.GetConcurDataAsync(accessToken, url);
                var serialized = JsonConvert.DeserializeObject<ExpensesHeaderItemization>(json);

                if (string.IsNullOrEmpty(serialized.Itemizations.NextPage))
                    hasData = false;
                else
                {
                    url = serialized.Itemizations.NextPage;
                }
                items.AddRange(serialized.Itemizations.Items.Itemization);
            }
            return items;
        }
        public async Task SendToBc_ExpensesHeader(List<Report> concurExpenses)
        {
            Console.WriteLine($"Starting to send {concurExpenses.Count} Expense Headers to Business Central...");
            bool hasData = true;
            var list = new List<BcExpenseHeader>();
            var requestUrl = _config["BusinessCentral:BC_Header_API"];
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC headers data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BcExpenseHeader>>(data);
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
            int total = 0, success = 0, failure = 0, added = 0, updated = 0;
            total = concurExpenses.Count;
            List<string> errors = new List<string>();
            foreach (var item in concurExpenses)
            {
                if (!list.Any(bc => bc.id.ToLower() == item.ID.ToLower()))
                {
                    string result = await _commonFunctions.SendDataToBcSingle(new BcExpenseHeader(item), requestUrl, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    added++;
                }
                else
                {
                    //update
                    var url = _config["BusinessCentral:BC_Header_API"];
                    string systemId = list.First(bc => bc.id.ToLower() == item.ID.ToLower()).SystemId;
                    url = url + $"(" + systemId + ")";
                    var result = await _commonFunctions.UpdateDataToBcSingle(new BcExpenseHeader(item), url, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    updated++;
                }
                //Console.WriteLine($"Processed {success + failure} of {total} records.");
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"ExpensesHeader: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");
        }

        public async Task SendToBc_ExpensesHeaderEntries(List<Entry> concurExpenses)
        {
            Console.WriteLine($"Starting to send {concurExpenses.Count} Expense Entries to Business Central...");
            bool hasData = true;
            var list = new List<BcExpenseHeaderEntry>();
            var requestUrl = _config["BusinessCentral:BC_Entries_API"];
            var bcTokenTime = DateTime.Now;
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC entries data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BcExpenseHeaderEntry>>(data);
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
            int total = 0, success = 0, failure = 0, added = 0, updated = 0;
            total = concurExpenses.Count;
            List<string> errors = new List<string>();
            foreach (var item in concurExpenses)
            {
                // Refresh BC token every 15 minutes
                if ((DateTime.Now - bcTokenTime).TotalMinutes > 30)
                {
                    bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
                    bcTokenTime = DateTime.Now;
                }
                if (!list.Any(bc => bc.id.ToLower() == item.ID.ToLower()))
                {
                    string result = await _commonFunctions.SendDataToBcSingle(new BcExpenseHeaderEntry(item), requestUrl, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    added++;
                }
                else
                {
                    //update
                    var url = _config["BusinessCentral:BC_Entries_API"];
                    string systemId = list.First(bc => bc.id.ToLower() == item.ID.ToLower()).SystemId;
                    url = url + $"(" + systemId + ")";
                    var result = await _commonFunctions.UpdateDataToBcSingle(new BcExpenseHeaderEntry(item), url, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    updated++;
                }
                //Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"BcExpenseHeaderEntry: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");
        }
        public async Task SendToBc_ExpensesHeaderCashAdvance(List<ExpenseCashAdvance> concurExpenses)
        {
            Console.WriteLine($"Starting to send {concurExpenses.Count} Expense Cash Advances to Business Central...");
            bool hasData = true;
            var list = new List<BC_ExpenseCashAdvance>();
            var requestUrl = _config["BusinessCentral:BC_Expense_CashAdv_API"];
            var bcTokenTime = DateTime.Now;
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC entries data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BC_ExpenseCashAdvance>>(data);
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
            int total = 0, success = 0, failure = 0, added = 0, updated = 0;
            total = concurExpenses.Count;
            List<string> errors = new List<string>();
            foreach (var item in concurExpenses)
            {
                // Refresh BC token every 15 minutes
                if ((DateTime.Now - bcTokenTime).TotalMinutes > 30)
                {
                    bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
                    bcTokenTime = DateTime.Now;
                }
                if (!list.Any(bc => bc.CashAdvanceId == item.CashAdvanceId))
                {
                    string result = await _commonFunctions.SendDataToBcSingle(new BC_ExpenseCashAdvance(item), requestUrl, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    added++;
                }
                else
                {
                    //update
                    var url = _config["BusinessCentral:BC_Expense_CashAdv_API"];
                    //string systemId = list.First(bc => bc.CashAdvanceId.ToLower() == item.CashAdvanceId.ToLower()).SystemId;
                    url = url + $"('" + item.CashAdvanceId + "')";
                    var result = await _commonFunctions.UpdateDataToBcSingle(new BC_ExpenseCashAdvance(item), url, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    updated++;
                }
                //Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"BcExpenseHeaderEntry: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");
        }
        public async Task SendToBc_ExpensesHeaderAllocations(List<ReportAllocation> concurExpenses)
        {
            Console.WriteLine($"Starting to send {concurExpenses.Count} Expense Header Allocations to Business Central...");
            bool hasData = true;
            var list = new List<BcExpenseHeaderAllocation>();
            var requestUrl = _config["BusinessCentral:BC_Expense_Alloc_API"];
            var bcTokenTime = DateTime.Now;
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            do
            {
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC entries data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BcExpenseHeaderAllocation>>(data);
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
            int total = 0, success = 0, failure = 0, added = 0, updated = 0;
            total = concurExpenses.Count;
            List<string> errors = new List<string>();
            foreach (var item in concurExpenses)
            {
                // Refresh BC token every 15 minutes
                if ((DateTime.Now - bcTokenTime).TotalMinutes > 30)
                {
                    bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
                    bcTokenTime = DateTime.Now;
                }
                if (!list.Any(bc => bc.Id.ToLower() == item.ID.ToLower()))
                {
                    string result = await _commonFunctions.SendDataToBcSingle(new BcExpenseHeaderAllocation(item), requestUrl, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    added++;
                }
                else
                {
                    //update
                    var url = _config["BusinessCentral:BC_Expense_Alloc_API"];
                    string systemId = list.First(bc => bc.Id.ToLower() == item.ID.ToLower()).SystemId;
                    url = url + $"(" + systemId + ")";
                    var result = await _commonFunctions.UpdateDataToBcSingle(new BcExpenseHeaderAllocation(item), url, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    updated++;
                }
                //Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"BcExpenseHeaderEntry: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");
        }

        public async Task SendToBc_ExpensesHeaderItemization(List<Itemization> concurExpenses)
        {
            Console.WriteLine($"Starting to send {concurExpenses.Count} Expense Itemizations to Business Central...");
            bool hasData = true;
            var list = new List<BcExpenseHeaderItemization>();
            var requestUrl = _config["BusinessCentral:BC_Itemization_API"];
            var bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
            var bcTokenTime = DateTime.Now;
            do
            {
                // Refresh BC token every 30 minutes
                if ((DateTime.Now - bcTokenTime).TotalMinutes > 30)
                {
                    bcToken = await _commonFunctions.GetBusinessCentralTokenAsync();
                    bcTokenTime = DateTime.Now;
                }
                var data = await _commonFunctions.GetExistingBcData(bcToken, requestUrl);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine("Failed to fetch existing BC itemizations data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BcExpenseHeaderItemization>>(data);
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
            int total = 0, success = 0, failure = 0, added = 0, updated = 0;
            total = concurExpenses.Count;
            List<string> errors = new List<string>();
            foreach (var item in concurExpenses)
            {
                if (!list.Any(bc => bc.id.ToLower() == item.ID.ToLower()))
                {
                    string result = await _commonFunctions.SendDataToBcSingle(new BcExpenseHeaderItemization(item), requestUrl, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    added++;
                }
                else
                {
                    //update
                    var url = _config["BusinessCentral:BC_Itemization_API"];
                    string systemId = list.First(bc => bc.id.ToLower() == item.ID.ToLower()).SystemId;
                    url = url + $"(" + systemId + ")";
                    var result = await _commonFunctions.UpdateDataToBcSingle(new BcExpenseHeaderItemization(item), url, bcToken);
                    if (result == "success")
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                        errors.Add(result);
                    }
                    updated++;
                }
                //Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"BcExpenseHeaderItemization: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");

        }
    }
}