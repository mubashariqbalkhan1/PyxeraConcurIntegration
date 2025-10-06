using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PyxeraConcurIntegrationConsole
{
    public class InvoiceService
    {
        private readonly IConfiguration _config;
        private readonly CommonFunctions _commonFunctions;

        public InvoiceService(HttpClient httpClient, IConfiguration config)
        {
            _commonFunctions = new CommonFunctions(httpClient, config);
            _config = config;
        }
        public async Task<List<PaymentRequest>> FetchInvoiceDigest()
        {
            string accessToken = await _commonFunctions.GetConcurAccessTokenAsync();
            string url = _config["Concur:Invoice_Digests"];
            var items = new List<PaymentRequestDigest>();
            var paymentrequest = new List<PaymentRequest>();
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
                var serialized = JsonConvert.DeserializeObject<PaymentRequestDigestsRoot>(json);

                if (string.IsNullOrEmpty(serialized.PaymentRequestDigests.NextPage))
                    hasData = false;
                else
                {
                    url = serialized.PaymentRequestDigests.NextPage;
                }
                items.AddRange(serialized.PaymentRequestDigests.PaymentRequestDigest);
            }
            Console.WriteLine($"Fetched {items.Count} items so far.");
            foreach (var item in items)
            {
                var pr = await FetchPaymentRequest(accessToken, item.ID);
                if (pr != null)
                {
                    paymentrequest.Add(pr);
                }
                Console.WriteLine($"Fetched {paymentrequest.Count} items so far.");
            }
            return paymentrequest;
        }

        public async Task<PaymentRequest> FetchPaymentRequest(string accessToken, string requestId)
        {
            string url = _config["Concur:Invoice_Detail"].Replace("{id}", requestId);
            var json = await _commonFunctions.GetConcurDataAsync(accessToken, url);
            var serialized = JsonConvert.DeserializeObject<PaymentRequestRoot>(json);
            return serialized.PaymentRequest;
        }

        public async Task SendToBc_InvoiceHeaders(List<PaymentRequest> concurExpenses)
        {
            bool hasData = true;
            var list = new List<BC_InvoiceHeader>();
            var requestUrl = _config["BusinessCentral:BC_Invoice_Header"];
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
                    Console.WriteLine("Failed to fetch existing BC invoice headers data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BC_InvoiceHeader>>(data);
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
                    string result = await _commonFunctions.SendDataToBcSingle(new BC_InvoiceHeader(item), requestUrl, bcToken);
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
                    var url = _config["BusinessCentral:BC_Invoice_Header"];
                    url = url + $"(" + list.First(bc => bc.id.ToLower() == item.ID.ToLower()).SystemId + ")";
                    var result = await _commonFunctions.UpdateDataToBcSingle(new BC_InvoiceHeader(item), url, bcToken);
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
                Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"✅ BC_Invoice_Header: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");

        }

        public async Task SendToBc_InvoiceLines(List<PaymentRequest> concurExpenses)
        {
            bool hasData = true;
            var list = new List<BC_InvoiceLineItem>();
            var requestUrl = _config["BusinessCentral:BC_Invoice_Lines"];
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
                    Console.WriteLine("Failed to fetch existing BC invoice lines data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BC_InvoiceLineItem>>(data);
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
            total = concurExpenses.Sum(pr => pr.LineItems?.LineItem.Count ?? 0);
            List<string> errors = new List<string>();
            var linesToSend = new List<BC_InvoiceLineItem>();
            foreach (var pr in concurExpenses)
            {
                try
                {
                    if (pr.LineItems == null)
                    {
                        Console.WriteLine($"Payment request {pr.ID} has no line items.");
                        continue;
                    }
                    foreach (var li in pr.LineItems.LineItem)
                    {
                        if (string.IsNullOrEmpty(li.LineItemId))
                        {
                            Console.WriteLine($"Line item ID is null or empty for line item of payment request {pr.ID}");
                            continue;
                        }
                        if (string.IsNullOrEmpty(pr.ID))
                        {
                            Console.WriteLine($"Payment request ID is null or empty for line item {li.LineItemId}");
                            continue;
                        }
                        if (!list.Any(bc => bc.lineItemId.ToLower() == li.LineItemId.ToLower() && bc.paymentRequestId.ToLower() == pr.ID.ToLower()))
                        {
                            var tosend = new BC_InvoiceLineItem(li, pr.ID);
                            string result = await _commonFunctions.SendDataToBcSingle(tosend, requestUrl, bcToken);
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
                            var url = _config["BusinessCentral:BC_Invoice_Lines"];
                            url = url + $"(" + list.First(bc => bc.lineItemId.ToLower() == li.LineItemId.ToLower() && bc.paymentRequestId.ToLower() == pr.ID.ToLower()).SystemId + ")";
                            var result = await _commonFunctions.UpdateDataToBcSingle(new BC_InvoiceLineItem(li, pr.ID), url, bcToken);
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
                        Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
                    }
                }
                catch
                {
                    Console.WriteLine(pr.LineItems);
                    Console.WriteLine($"Failed to map line item of payment request {pr.ID}");
                }
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"✅ BC_Invoice_Header: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");
        }

        public async Task SendToBc_InvoiceLineAllocations(List<PaymentRequest> concurExpenses)
        {
            bool hasData = true;
            var list = new List<BC_Invoice_Allocations>();
            var requestUrl = _config["BusinessCentral:BC_Invoice_Allocations"];
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
                    Console.WriteLine("Failed to fetch existing BC invoice allocations data.");
                    return;
                }
                var parsedBcData = JsonConvert.DeserializeObject<Root<BC_Invoice_Allocations>>(data);
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
            total = concurExpenses.Sum(pr => pr.LineItems?.LineItem.Sum(li => li.Allocations?.Allocation.Count ?? 0) ?? 0);
            List<string> errors = new List<string>();
            var linesToSend = new List<BC_Invoice_Allocations>();
            foreach (var item in concurExpenses)
            {
                if (item.LineItems == null)
                {
                    Console.WriteLine($"Payment request {item.ID} has no line items.");
                    continue;
                }
                foreach (var li in item.LineItems.LineItem)
                {
                    if (li.Allocations == null || li.Allocations.Allocation == null || li.Allocations.Allocation.Count == 0)
                    {
                        Console.WriteLine($"Line item {li.LineItemId} of payment request {item.ID} has no allocations.");
                        continue;
                    }
                    foreach (var ac in li.Allocations.Allocation)
                    {
                        if (!list.Any(bc => bc.lineItemReference.ToLower() == li.LineItemId.ToLower() && bc.paymentRequestId.ToLower() == item.ID.ToLower()))
                        {
                            var tosend = new BC_Invoice_Allocations(ac, li.LineItemId, item.ID);
                            string result = await _commonFunctions.SendDataToBcSingle(tosend, requestUrl, bcToken);
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
                            var url = _config["BusinessCentral:BC_Invoice_Allocations"];
                            url = url + $"(" + list.First(bc => bc.lineItemReference.ToLower() == li.LineItemId.ToLower() && bc.paymentRequestId.ToLower() == item.ID.ToLower()).SystemId + ")";
                            var result = await _commonFunctions.UpdateDataToBcSingle(new BC_Invoice_Allocations(ac, li.LineItemId, item.ID), url, bcToken);
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
                        Console.WriteLine($"☑️ Processed {success + failure} of {total} records.");
                    }
                }
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"✅ BC_Invoice_Header: Sent {total} records → Success: {success}, Failure: {failure}, Added: {added}, Updated: {updated}");
        }
    }
}