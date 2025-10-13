using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PyxeraConcurIntegrationConsole
{
    public class CommonFunctions
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public CommonFunctions(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<string> GetConcurAccessTokenAsync()
        {
            string url = _config["Concur:Token_URL"];
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", _config["Concur:CLIENT_ID"]),
                    new KeyValuePair<string, string>("client_secret", _config["Concur:CLIENT_SECRET"]),
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", _config["Concur:REFRESH_TOKEN"]),
                })
            };

            var response = await _httpClient.SendAsync(tokenRequest);
            var tokenJson = await response.Content.ReadAsStringAsync();
            dynamic tokenObj = JsonConvert.DeserializeObject(tokenJson);
            return tokenObj?.access_token;
        }

        public async Task<string> GetBusinessCentralTokenAsync()
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post,
                $"https://login.microsoftonline.com/{_config["BusinessCentral:BC_TENANT_ID"]}/oauth2/v2.0/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", _config["BusinessCentral:BC_CLIENT_ID"]),
                    new KeyValuePair<string, string>("scope", "https://api.businesscentral.dynamics.com/.default"),
                    new KeyValuePair<string, string>("client_secret", _config["BusinessCentral:BC_CLIENT_SECRET"]),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                })
            };

            var response = await _httpClient.SendAsync(tokenRequest);
            var tokenJson = await response.Content.ReadAsStringAsync();
            dynamic tokenObj = JsonConvert.DeserializeObject(tokenJson);
            return tokenObj?.access_token;
        }
        public async Task<string> GetConcurDataAsync(string accessToken, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var xmlContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed Concur fetch. Message: {response.ReasonPhrase}");
                Console.WriteLine(url);
                return "";
            }

            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(xmlContent);
            return JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
        }
        public async Task<string> GetConcurDataJsonAsync(string accessToken, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var xmlContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed Concur fetch. Message: {response.ReasonPhrase}");
                Console.WriteLine(url);
                return "";
            }
            return xmlContent;
        }
        public async Task<string> GetConcurStringDataAsync(string accessToken, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var xmlContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed Concur fetch. Message: {response.ReasonPhrase}");
                Console.WriteLine(url);
                return "";
            }

            return xmlContent;
        }
        #region Get Existing Business Central Data
        public async Task<string> GetExistingBcData(string bcToken, string requestUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bcToken);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch existing BC data. Status: {response.StatusCode}");
                return "";
            }
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
        public async Task SendDataToBc<T>(IEnumerable<T> dataObjects, string requestUrl, string bcToken, string calledFrom)
        {
            Console.WriteLine($"Starting to send {dataObjects.Count()} {calledFrom} to Business Central...");
            int success = 0, failure = 0, total = dataObjects.Count();
            List<string> errors = new List<string>();
            foreach (var obj in dataObjects)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                    };
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bcToken);
                    var response = await _httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        failure++;
                        Console.WriteLine($"BC send failed. Status: {response.StatusCode}, Response: {content}");
                        errors.Add($"Error: {response.StatusCode}, Message: {content}, Object: {JsonConvert.SerializeObject(obj)}");
                    }
                    else
                    {
                        success++;
                    }
                }
                catch (Exception ex)
                {
                    failure++;
                    errors.Add($"Error: {ex.Message}, Object: {JsonConvert.SerializeObject(obj)}");
                }
            }
            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"📌{calledFrom}: Sent {total} records → Success: {success}, Failure: {failure}");
        }
        public async Task<string> SendDataToBcSingle<T>(T obj, string requestUrl, string bcToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bcToken);
                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"BC send failed. Status: {response.StatusCode}, Response: {content}");
                    return $"Error: {response.StatusCode}, Message: {content}";
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> UpdateDataToBcSingle<T>(T obj, string requestUrl, string bcToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bcToken);
                request.Headers.Add("If-Match", "*");

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"BC send failed. Status: {response.StatusCode}, Response: {content}");
                    return $"Error: {response.StatusCode}, Message: {content}";
                }
                return "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Object: {JsonConvert.SerializeObject(obj)}");
                return ex.Message;
            }
        }
        public async Task UpdateDataToBc<T>(IEnumerable<T> dataObjects, string requestUrl, string bcToken, string calledFrom)
        {
            int success = 0, failure = 0, total = dataObjects.Count();
            List<string> errors = new List<string>();

            foreach (var obj in dataObjects)
            {
                try
                {
                    // ✅ Get SystemId from object (using reflection)
                    var systemIdProp = obj?.GetType().GetProperty("SystemId");
                    var systemIdValue = systemIdProp?.GetValue(obj)?.ToString();

                    // Build request URL with SystemId if available
                    var finalUrl = string.IsNullOrWhiteSpace(systemIdValue)
                        ? requestUrl
                        : $"{requestUrl.TrimEnd('/')}/{systemIdValue}";

                    var request = new HttpRequestMessage(HttpMethod.Post, finalUrl)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                    };
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bcToken);

                    var response = await _httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        failure++;
                        Console.WriteLine($"BC send failed. Status: {response.StatusCode}, Response: {content}");
                        errors.Add($"Error: {response.StatusCode}, Message: {content}, Object: {JsonConvert.SerializeObject(obj)}");
                    }
                    else
                    {
                        success++;
                    }

                    Console.WriteLine($"Processed total: {success + failure}, out of {total}, remaining: {total - (success + failure)}");
                }
                catch (Exception ex)
                {
                    failure++;
                    errors.Add($"Error: {ex.Message}, Object: {JsonConvert.SerializeObject(obj)}");
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine, errors));
            Console.WriteLine($"📌{calledFrom}: Sent {total} records → Success: {success}, Failure: {failure}");
        }


    }
    public class Customs
    {
        public CustomField? Custom1 { get; set; }
        public CustomField? Custom2 { get; set; }
        public CustomField? Custom3 { get; set; }
        public CustomField? Custom4 { get; set; }
        public CustomField? Custom5 { get; set; }
        public CustomField? Custom6 { get; set; }
        public CustomField? Custom7 { get; set; }
        public CustomField? Custom8 { get; set; }
        public CustomField? Custom9 { get; set; }
        public CustomField? Custom10 { get; set; }
        public CustomField? Custom11 { get; set; }
        public CustomField? Custom12 { get; set; }
        public CustomField? Custom13 { get; set; }
        public CustomField? Custom14 { get; set; }
        public CustomField? Custom15 { get; set; }
        public CustomField? Custom16 { get; set; }
        public CustomField? Custom17 { get; set; }
        public CustomField? Custom18 { get; set; }
        public CustomField? Custom19 { get; set; }
        public CustomField? Custom20 { get; set; }
        public CustomField? Custom21 { get; set; }
        public CustomField? Custom22 { get; set; }
        public CustomField? Custom23 { get; set; }
        public CustomField? Custom24 { get; set; }
        public CustomField? Custom25 { get; set; }
        public CustomField? Custom26 { get; set; }
        public CustomField? Custom27 { get; set; }
        public CustomField? Custom28 { get; set; }
        public CustomField? Custom29 { get; set; }
        public CustomField? Custom30 { get; set; }
        public CustomField? Custom31 { get; set; }
        public CustomField? Custom32 { get; set; }
        public CustomField? Custom33 { get; set; }
        public CustomField? Custom34 { get; set; }
        public CustomField? Custom35 { get; set; }
        public CustomField? Custom36 { get; set; }
        public CustomField? Custom37 { get; set; }
        public CustomField? Custom38 { get; set; }
        public CustomField? Custom39 { get; set; }
        public CustomField? Custom40 { get; set; }
    }
    public class Customs2
    {
        [XmlElement("Custom1")]
        public CustomField2? Custom1 { get; set; }
        [XmlElement("Custom2")]
        public CustomField2? Custom2 { get; set; }
        [XmlElement("Custom3")]
        public CustomField2? Custom3 { get; set; }
        [XmlElement("Custom4")]
        public CustomField2? Custom4 { get; set; }
        [XmlElement("Custom5")]
        public CustomField2? Custom5 { get; set; }
        [XmlElement("Custom6")]
        public CustomField2? Custom6 { get; set; }
        [XmlElement("Custom7")]
        public CustomField2? Custom7 { get; set; }
        [XmlElement("Custom8")]
        public CustomField2? Custom8 { get; set; }
        [XmlElement("Custom9")]
        public CustomField2? Custom9 { get; set; }
        [XmlElement("Custom10")]
        public CustomField2? Custom10 { get; set; }
        [XmlElement("Custom11")]
        public CustomField2? Custom11 { get; set; }
        [XmlElement("Custom12")]
        public CustomField2? Custom12 { get; set; }
        [XmlElement("Custom13")]
        public CustomField2? Custom13 { get; set; }
        [XmlElement("Custom14")]
        public CustomField2? Custom14 { get; set; }
        [XmlElement("Custom15")]
        public CustomField2? Custom15 { get; set; }
        [XmlElement("Custom16")]
        public CustomField2? Custom16 { get; set; }
        [XmlElement("Custom17")]
        public CustomField2? Custom17 { get; set; }
        [XmlElement("Custom18")]
        public CustomField2? Custom18 { get; set; }
        [XmlElement("Custom19")]
        public CustomField2? Custom19 { get; set; }
        [XmlElement("Custom20")]
        public CustomField2? Custom20 { get; set; }
    }

    public class Root<T>
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<T> value { get; set; }
        [JsonProperty("@odata.nextLink")]
        public string odatanextLink { get; set; }

    }
    public class NilObject
    {
        [JsonProperty("@i:nil")]
        public string nil { get; set; }
    }
}
