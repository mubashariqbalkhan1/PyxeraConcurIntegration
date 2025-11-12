// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Globalization;
using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class ExpensesHeaderReport
    {
        public Reports Reports { get; set; }
    }
    public class Reports
    {
        [JsonProperty("@xmlns:xsd")]
        public string xmlnsxsd { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }
        public ReportsHeader Items { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string NextPage { get; set; }
    }
    public class ReportsHeader
    {
        public List<Report> Report { get; set; }
    }
    public class Report : Customs
    {
        public string ID { get; set; }
        public string URI { get; set; }
        public string Name { get; set; }
        public string Total { get; set; }
        public string CurrencyCode { get; set; }
        public string Country { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string CountrySubdivision { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset CreateDate { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset SubmitDate { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? ProcessingPaymentDate { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? PaidDate { get; set; }
        public string ReceiptsReceived { get; set; }
        public DateTime UserDefinedDate { get; set; }
        public string LastComment { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string OwnerLoginID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string OwnerName { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string ApproverLoginID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string ApproverName { get; set; }

        public string ApprovalStatusName { get; set; }
        public string ApprovalStatusCode { get; set; }
        public string PaymentStatusName { get; set; }
        public string PaymentStatusCode { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string PersonalAmount { get; set; }
        public string AmountDueEmployee { get; set; }
        public string AmountDueCompanyCard { get; set; }
        public string TotalClaimedAmount { get; set; }
        public string TotalApprovedAmount { get; set; }
        public string LedgerName { get; set; }
        public string PolicyID { get; set; }
        public string EverSentBack { get; set; }
        public string HasException { get; set; }
        public string WorkflowActionUrl { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string OrgUnit1 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string OrgUnit2 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string OrgUnit3 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string OrgUnit4 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string OrgUnit5 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string OrgUnit6 { get; set; }
    }

    public class BcExpenseHeader
    {
        public BcExpenseHeader()
        {

        }
        public BcExpenseHeader(Report report)
        {
            id = report.ID;
            name = report.Name;
            total = Decimal.Parse(report.Total);
            currencyCode = report.CurrencyCode;
            processingPaymentDate = report.ProcessingPaymentDate.HasValue ? DateOnly.FromDateTime(report.ProcessingPaymentDate.Value.DateTime) : null;
            paidDate = report.PaidDate.HasValue ? DateOnly.FromDateTime(report.PaidDate.Value.DateTime) : null;
            approvalStatusName = report.ApprovalStatusName;
            approvalStatusCode = report.ApprovalStatusCode;
            paymentStatusName = report.PaymentStatusName;
            paymentStatusCode = report.PaymentStatusCode;
            personalAmount = Decimal.Parse(report.PersonalAmount);
            amountDueEmployee = Decimal.Parse(report.AmountDueEmployee);
            amountDueCompany = Decimal.Parse(report.AmountDueCompanyCard);
            totalClaimedAmount = Decimal.Parse(report.TotalClaimedAmount);
            totalApprovedAmount = Decimal.Parse(report.TotalApprovedAmount);
            employeeId = report.Custom10.Value;
            submitDate = DateOnly.FromDateTime(report.SubmitDate.DateTime);
            createDate = DateOnly.FromDateTime(report.CreateDate.DateTime);
            if (string.IsNullOrEmpty(report.Custom7.Value))
            {
                postingPeriod = null;
            }
            else
            {
                if (DateTime.TryParseExact(report.Custom7.Code, "MMddyyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime pd))
                {
                    postingPeriod = DateOnly.FromDateTime(pd);
                }
                else
                {
                    postingPeriod = null;
                }
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public decimal total { get; set; }
        public string currencyCode { get; set; }
        public DateOnly? processingPaymentDate { get; set; }
        public DateOnly? paidDate { get; set; }
        public string approvalStatusName { get; set; }
        public string approvalStatusCode { get; set; }
        public string paymentStatusName { get; set; }
        public string paymentStatusCode { get; set; }
        public decimal personalAmount { get; set; }
        public decimal amountDueEmployee { get; set; }
        public decimal amountDueCompany { get; set; }
        public decimal totalClaimedAmount { get; set; }
        public decimal totalApprovedAmount { get; set; }
        public string employeeId { get; set; }
        public DateOnly? submitDate { get; set; }
        public DateOnly? createDate { get; set; }
        public DateOnly? postingPeriod { get; set; }
    }

}