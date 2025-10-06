using Newtonsoft.Json;

namespace PyxeraConcurIntegrationConsole
{
    public class ExpensesHeaderItemization
    {
        public Itemizations Itemizations { get; set; }
    }
    public class Itemizations
    {
        [JsonProperty("@xmlns:xsd")]
        public string xmlnsxsd { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }
        public ItemsItemization Items { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string NextPage { get; set; }
    }
    public class ItemsItemization
    {
        public List<Itemization> Itemization { get; set; }
    }
    public class Itemization : Customs
    {
        public string ID { get; set; }
        public string URI { get; set; }
        public string EntryID { get; set; }
        public string ReportID { get; set; }
        public string ReportOwnerID { get; set; }
        public string ExpenseTypeCode { get; set; }
        public string ExpenseTypeName { get; set; }
        public string SpendCategoryCode { get; set; }
        public string SpendCategoryName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal PostedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? LocationID { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? LocationName { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? LocationSubdivision { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? LocationCountry { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Description { get; set; }

        public bool IsPersonal { get; set; }
        public bool IsBillable { get; set; }
        public bool IsImageRequired { get; set; }
        public string AllocationType { get; set; }
        public bool HasComments { get; set; }
        public bool HasExceptions { get; set; }
        public DateTime LastModified { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OrgUnit1 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OrgUnit2 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OrgUnit3 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OrgUnit4 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OrgUnit5 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OrgUnit6 { get; set; }

        
    }

    public class BcExpenseHeaderItemization
    {
        public BcExpenseHeaderItemization()
        {

        }
        public BcExpenseHeaderItemization(Itemization itemization)
        {
            id = itemization.ID;
            entryId = itemization.EntryID;
            reportId = itemization.ReportID;
            reportOwnerId = itemization.ReportOwnerID;
            expenseTypeCode = itemization.ExpenseTypeCode;
            expenseTypeName = itemization.ExpenseTypeName;
            spendCategoryCode = itemization.SpendCategoryCode;
            spendCategoryName = itemization.SpendCategoryName;
            transactionDate = itemization.TransactionDate.ToUniversalTime();
            transactionAmount = (int)itemization.TransactionAmount;
            postedAmount = (int)itemization.PostedAmount;
            approvedAmount = (int)itemization.ApprovedAmount;
            location = itemization.LocationSubdivision;
            locationID = itemization.LocationID;
            locationCountry = itemization.LocationCountry;
            descrioption = itemization.Description;
            isPersonal = itemization.IsPersonal;
            isBillable = itemization.IsBillable;
            isImageRequired = itemization.IsImageRequired;
            allocationType = itemization.AllocationType;
            hasComments = itemization.HasComments;
            hasExceptions = itemization.HasExceptions;

            detailedDescription = itemization.Custom1.Value;
            departmentCode = itemization.Custom2.Code;
            departmentName = itemization.Custom2.Value;
            programCode = itemization.Custom4.Code;
            programName = itemization.Custom4.Value;
            activityCode = itemization.Custom5.Code;
            activityName = itemization.Custom5.Value;
            locationCode = itemization.Custom6.Code;
            locationName = itemization.Custom6.Value;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public string id { get; set; }
        public string entryId { get; set; }
        public string reportId { get; set; }
        public string reportOwnerId { get; set; }
        public string expenseTypeCode { get; set; }
        public string expenseTypeName { get; set; }
        public string spendCategoryCode { get; set; }
        public string spendCategoryName { get; set; }
        public DateTime transactionDate { get; set; }
        public int transactionAmount { get; set; }
        public int postedAmount { get; set; }
        public int approvedAmount { get; set; }
        public string location { get; set; }
        public string locationID { get; set; }
        public string locationCountry { get; set; }
        public string descrioption { get; set; }
        public bool isPersonal { get; set; }
        public bool isBillable { get; set; }
        public bool isImageRequired { get; set; }
        public string allocationType { get; set; }
        public bool hasComments { get; set; }
        public bool hasExceptions { get; set; }
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
