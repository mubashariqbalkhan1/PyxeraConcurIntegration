// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class ExpensesHeaderReportAllocation
    {
        public ReportsAllocation Allocations { get; set; }
    }
    public class ReportsAllocation
    {
        [JsonProperty("@xmlns:xsd")]
        public string xmlnsxsd { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }
        public ReportsHeaderAllocation Items { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string NextPage { get; set; }
    }
    public class ReportsHeaderAllocation
    {
        public List<ReportAllocation> Allocation { get; set; }
    }
    public class ReportAllocation : Customs2
    {
        public string ID { get; set; }
        public string URI { get; set; }
        public string EntryID { get; set; }
        public decimal Percentage { get; set; }
        public bool IsPercentEdited { get; set; }
        public bool IsHidden { get; set; }
        [JsonConverter(typeof(NullReplacementConverterI))]
        public string AccountCode1 { get; set; }
        [JsonConverter(typeof(NullReplacementConverterI))]
        public string AccountCode2 { get; set; }
    }

    public class BcExpenseHeaderAllocation
    {
        public BcExpenseHeaderAllocation()
        {

        }
        public BcExpenseHeaderAllocation(ReportAllocation report)
        {
            this.Id = report.ID;
            this.EntryId = report.EntryID;
            this.AccountCode = report.AccountCode1;
            this.Percentage = report.Percentage;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public string Id { get; set; }
        public string EntryId { get; set; }
        public string AccountCode { get; set; }
        public decimal Percentage { get; set; }
    }
}