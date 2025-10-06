using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class ExpensesHeaderEntries
    {
        public Entries Entries { get; set; }
    }
    public class Entries
    {
        [JsonProperty("@xmlns:xsd")]
        public string xmlnsxsd { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }
        public ItemsEntry Items { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string NextPage { get; set; }
    }
    public class ItemsEntry
    {
        public List<Entry> Entry { get; set; }
    }
    public class Entry : Customs
    {
        // Simple properties
        public string ID { get; set; }
        public string URI { get; set; }
        public string ExpenseID { get; set; }
        public string ReportID { get; set; }
        public string ReportOwnerID { get; set; }
        public string ExpenseTypeCode { get; set; }
        public string ExpenseTypeName { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string SpendCategoryCode { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string SpendCategoryName { get; set; }
        public string PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal PostedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public bool IsPersonal { get; set; }
        public bool IsBillable { get; set; }
        public bool IsPersonalCardCharge { get; set; }
        public bool HasImage { get; set; }
        public bool IsImageRequired { get; set; }
        public bool ReceiptReceived { get; set; }
        public string TaxReceiptType { get; set; }
        public bool HasItemizations { get; set; }
        public string AllocationType { get; set; }
        public bool HasAttendees { get; set; }
        public bool HasVAT { get; set; }
        public bool HasAppliedCashAdvance { get; set; }
        public bool HasComments { get; set; }
        public bool HasExceptions { get; set; }
        public bool IsPaidByExpensePay { get; set; }
        public DateTime LastModified { get; set; }
        public string FormID { get; set; }

        // Properties that can have {"xsi:nil": "true"}
        [JsonConverter(typeof(NullReplacementConverter))]
        public string VendorDescription { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string VendorListItemID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string VendorListItemName { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string ExpenseTypeListItemID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string ExpenseTypeListItemName { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string LocationID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string LocationName { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string LocationSubdivision { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string LocationCountry { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string Description { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string ElectronicReceiptID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string CompanyCardTransactionID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string TripID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string EmployeeBankAccountID { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string Journey { get; set; }
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

    public class CustomField
    {
        public string Type { get; set; }
        public string Value { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string Code { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string ListItemID { get; set; }
    }

    public class BcExpenseHeaderEntry
    {
        public BcExpenseHeaderEntry()
        {

        }
        public BcExpenseHeaderEntry(Entry entry)
        {
            id = entry.ID;
            expenseId = entry.ExpenseID;
            reportId = entry.ReportID;
            reportOwnerId = entry.ReportOwnerID;
            expenseTypeCode = entry.ExpenseTypeCode;
            expenseTypeName = entry.ExpenseTypeName;
            paymentTypeId = entry.PaymentTypeID;
            paymentTypeName = entry.PaymentTypeName;
            transactionDate = entry.TransactionDate.ToUniversalTime();
            transactionCurrencyCode = entry.TransactionCurrencyCode;
            transactionAmount = entry.TransactionAmount;
            exchangeRate = entry.ExchangeRate;
            postedAmount = entry.PostedAmount;
            approvedAmount = entry.ApprovedAmount;
            vendorDescription = entry.VendorDescription;
            vendorListItemId = entry.VendorListItemID;
            vendorListItemName = entry.VendorListItemName;
            isPersonal = entry.IsPersonal;
            isPersonalCardCharge = entry.IsPersonalCardCharge;
            companyCardTransId = entry.CompanyCardTransactionID;
            tripId = entry.TripID;
            hasItemization = entry.HasItemizations;
            hasAppCashAdv = entry.HasAppliedCashAdvance;
            hasComments = entry.HasComments;
            hasException = entry.HasExceptions;
            isPaidByExpPay = entry.IsPaidByExpensePay;
            detailedDescription = entry.Custom1.Value;
            departmentCode = entry.Custom2.Code;
            departmentName = entry.Custom2.Value;
            programCode = entry.Custom4.Code;
            programName = entry.Custom4.Value;
            activityCode = entry.Custom5.Code;
            activityName = entry.Custom5.Value;
            locationCode = entry.Custom6.Code;
            locationName = entry.Custom6.Value;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public string id { get; set; }
        public string expenseId { get; set; }
        public string reportId { get; set; }
        public string reportOwnerId { get; set; }
        public string expenseTypeCode { get; set; }
        public string expenseTypeName { get; set; }
        public string paymentTypeId { get; set; }
        public string paymentTypeName { get; set; }
        public DateTime transactionDate { get; set; }
        public string transactionCurrencyCode { get; set; }
        public decimal transactionAmount { get; set; }
        public decimal exchangeRate { get; set; }
        public decimal postedAmount { get; set; }
        public decimal approvedAmount { get; set; }
        public string vendorDescription { get; set; }
        public string vendorListItemId { get; set; }
        public string vendorListItemName { get; set; }
        public bool isPersonal { get; set; }
        public bool isPersonalCardCharge { get; set; }
        public string companyCardTransId { get; set; }
        public string tripId { get; set; }
        public bool hasItemization { get; set; }
        public bool hasAppCashAdv { get; set; }
        public bool hasComments { get; set; }
        public bool hasException { get; set; }
        public bool isPaidByExpPay { get; set; }
        public string detailedDescription { get; set; }
        public string departmentCode { get; set; }
        public string departmentName { get; set; }
        public string programCode { get; set; }
        public string programName { get; set; }
        public string activityCode { get; set; }
        public string activityName { get; set; }
        public string locationCode { get; set; }
        public string locationName { get; set; }
    }

}