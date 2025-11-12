using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class BrexMasterCard
    {
        public DateTime? DATE { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string ReportID { get; set; }
        public string EmployeeDefaultCurrencyAlphaCode { get; set; }
        public DateTime? ReportSubmitDate { get; set; }
        public DateTime? ReportProcessingPaymentDate { get; set; }
        public string ReportName { get; set; }
        public string ReportEntryExpenseTypeName { get; set; }
        public DateTime? ReportEntryTransactionDate { get; set; }
        public string ReportEntryIsPersonalFlag { get; set; }
        public string ReportEntryDescription { get; set; }
        public string ReportEntryVendorName { get; set; }
        public string ReportEntryVendorDescription { get; set; }
        public string ReportEntryPaymentCodeCode { get; set; }
        public string ReportEntryPaymentCodeName { get; set; }
        public string ReportPaymentReimbursementType { get; set; }
        public string BilledCreditCardAccountNumber { get; set; }
        public string BilledCreditCardAccountDescription { get; set; }
        public string JournalPayerPaymentCodeName { get; set; }
        public string JournalPayeePaymentTypeName { get; set; }
        public decimal JournalAmount { get; set; }
        public string JournalAccountCode { get; set; }
        public string JournalDebitOrCredit { get; set; }
        public string PaymentDemandCompanyCashAccountCode { get; set; }
        public string PaymentDemandCompanyLiabilityAccountCode { get; set; }
        public DateTime? EstimatedPaymentDate { get; set; }
        public string Program { get; set; }
        public string Department { get; set; }
        public string AllocationCustom6 { get; set; }
        public string Activity { get; set; }
        public string AdditionalExplanation { get; set; }
        public string AccountString { get; set; }
        public string ReportPurpose { get; set; }
    }
    public class PostBrexMaster
    {
        public PostBrexMaster()
        {

        }
        public PostBrexMaster(BrexMasterCard brexMasterCard)
        {
            this.Activity = brexMasterCard.Activity;
            this.AdditionalExplanation = brexMasterCard.AdditionalExplanation;
            this.AllocationCustom6 = brexMasterCard.AllocationCustom6;
            this.BilledCreditCardAccDescr = brexMasterCard.BilledCreditCardAccountDescription;
            this.BilledCreditCardAccNo = brexMasterCard.BilledCreditCardAccountNumber;
            this.Date = brexMasterCard.DATE != null ? DateOnly.FromDateTime(brexMasterCard.DATE.Value) : null;
            this.Department = brexMasterCard.Department;
            this.EmployeeDefCurrAlphaCode = brexMasterCard.EmployeeDefaultCurrencyAlphaCode;
            this.EmployeeFirstName = brexMasterCard.EmployeeFirstName;
            this.EmployeeId = brexMasterCard.EmployeeID;
            this.EmployeeLastName = brexMasterCard.EmployeeLastName;
            this.EstimatedPaymentDate = brexMasterCard.EstimatedPaymentDate != null ? DateOnly.FromDateTime(brexMasterCard.EstimatedPaymentDate.Value) : null;
            this.JnlPayeePaymentTypeName = brexMasterCard.JournalPayeePaymentTypeName;
            this.JnlPayerPaymentCodeName = brexMasterCard.JournalPayerPaymentCodeName;
            this.JournalAccountCode = brexMasterCard.JournalAccountCode;
            this.JournalAmount = brexMasterCard.JournalAmount;
            this.JournalDebitOrCredit = brexMasterCard.JournalDebitOrCredit;
            this.PayDemCpnyCashAccCode = brexMasterCard.PaymentDemandCompanyCashAccountCode;
            this.PayDemCpnyLBAccCode = brexMasterCard.PaymentDemandCompanyLiabilityAccountCode;
            this.Program = brexMasterCard.Program;
            this.ReportEntryDescription = brexMasterCard.ReportEntryDescription;
            this.ReportEntryExpenseTypeName = brexMasterCard.ReportEntryExpenseTypeName;
            this.ReportEntryIsPersonalFlag = brexMasterCard.ReportEntryIsPersonalFlag;
            this.ReportEntryPaymentCode = brexMasterCard.ReportEntryPaymentCodeCode;
            this.ReportEntryPaymentCodeName = brexMasterCard.ReportEntryPaymentCodeName;
            this.ReportEntryTransactionDate = brexMasterCard.ReportEntryTransactionDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportEntryTransactionDate.Value) : null;
            this.ReportEntryVendorDescr = brexMasterCard.ReportEntryVendorDescription;
            this.ReportEntryVendorName = brexMasterCard.ReportEntryVendorName;
            this.ReportId = brexMasterCard.ReportID;
            this.ReportName = brexMasterCard.ReportName;
            this.ReportPaymentReimbType = brexMasterCard.ReportPaymentReimbursementType;
            this.ReportProcessingPaymentDate = brexMasterCard.ReportProcessingPaymentDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportProcessingPaymentDate.Value) : null;
            this.ReportSubmitDate = brexMasterCard.ReportSubmitDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportSubmitDate.Value) : null;
        }
        public int EntryNo { get; set; }
        public string Activity { get; set; }
        public string AdditionalExplanation { get; set; }
        public string AllocationCustom6 { get; set; }
        public string BilledCreditCardAccDescr { get; set; }
        public string BilledCreditCardAccNo { get; set; }
        public DateOnly? Date { get; set; }
        public string Department { get; set; }
        public string EmployeeDefCurrAlphaCode { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeLastName { get; set; }
        public DateOnly? EstimatedPaymentDate { get; set; }
        public string JnlPayeePaymentTypeName { get; set; }
        public string JnlPayerPaymentCodeName { get; set; }
        public string JournalAccountCode { get; set; }
        public decimal JournalAmount { get; set; }
        public string JournalDebitOrCredit { get; set; }
        public string PayDemCpnyCashAccCode { get; set; }
        public string PayDemCpnyLBAccCode { get; set; }
        public string Program { get; set; }
        public string ReportEntryDescription { get; set; }
        public string ReportEntryExpenseTypeName { get; set; }
        public string ReportEntryIsPersonalFlag { get; set; }
        public string ReportEntryPaymentCode { get; set; }
        public string ReportEntryPaymentCodeName { get; set; }
        public DateOnly? ReportEntryTransactionDate { get; set; }
        public string ReportEntryVendorDescr { get; set; }
        public string ReportEntryVendorName { get; set; }
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportPaymentReimbType { get; set; }
        public DateOnly? ReportProcessingPaymentDate { get; set; }
        public DateOnly? ReportSubmitDate { get; set; }
    }
    public class PostCompanyCheck
    {
        public PostCompanyCheck()
        {

        }
        public PostCompanyCheck(BrexMasterCard brexMasterCard)
        {
            this.Activity = brexMasterCard.Activity;
            this.AdditionalExplanation = brexMasterCard.AdditionalExplanation;
            this.AllocationCustom6 = brexMasterCard.AllocationCustom6;
            this.BilledCreditCardAccDescr = brexMasterCard.BilledCreditCardAccountDescription;
            this.BilledCreditCardAccNo = brexMasterCard.BilledCreditCardAccountNumber;
            this.Date = brexMasterCard.DATE != null ? DateOnly.FromDateTime(brexMasterCard.DATE.Value) : null;
            this.Department = brexMasterCard.Department;
            this.EmployeeDefCurrAlphaCode = brexMasterCard.EmployeeDefaultCurrencyAlphaCode;
            this.EmployeeFirstName = brexMasterCard.EmployeeFirstName;
            this.EmployeeId = brexMasterCard.EmployeeID;
            this.EmployeeLastName = brexMasterCard.EmployeeLastName;
            this.EstimatedPaymentDate = brexMasterCard.EstimatedPaymentDate != null ? DateOnly.FromDateTime(brexMasterCard.EstimatedPaymentDate.Value) : null;
            this.JnlPayeePaymentTypeName = brexMasterCard.JournalPayeePaymentTypeName;
            this.JnlPayerPaymentCodeName = brexMasterCard.JournalPayerPaymentCodeName;
            this.JournalAccountCode = brexMasterCard.JournalAccountCode;
            this.JournalAmount = brexMasterCard.JournalAmount;
            this.JournalDebitOrCredit = brexMasterCard.JournalDebitOrCredit;
            this.PayDemCpnyCashAccCode = brexMasterCard.PaymentDemandCompanyCashAccountCode;
            this.PayDemCpnyLBAccCode = brexMasterCard.PaymentDemandCompanyLiabilityAccountCode;
            this.Program = brexMasterCard.Program;
            this.ReportEntryDescription = brexMasterCard.ReportEntryDescription;
            this.ReportEntryExpenseTypeName = brexMasterCard.ReportEntryExpenseTypeName;
            this.ReportEntryIsPersonalFlag = brexMasterCard.ReportEntryIsPersonalFlag;
            this.ReportEntryPaymentCode = brexMasterCard.ReportEntryPaymentCodeCode;
            this.ReportEntryPaymentCodeName = brexMasterCard.ReportEntryPaymentCodeName;
            this.ReportEntryTransactionDate = brexMasterCard.ReportEntryTransactionDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportEntryTransactionDate.Value) : null;
            this.ReportEntryVendorDescr = brexMasterCard.ReportEntryVendorDescription;
            this.ReportEntryVendorName = brexMasterCard.ReportEntryVendorName;
            this.ReportId = brexMasterCard.ReportID;
            this.ReportName = brexMasterCard.ReportName;
            this.ReportPaymentReimbType = brexMasterCard.ReportPaymentReimbursementType;
            this.ReportProcessingPaymentDate = brexMasterCard.ReportProcessingPaymentDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportProcessingPaymentDate.Value) : null;
            this.ReportSubmitDate = brexMasterCard.ReportSubmitDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportSubmitDate.Value) : null;
            this.accountString = brexMasterCard.AccountString;
            this.reportPurpose = brexMasterCard.ReportPurpose;
        }
        public int EntryNo { get; set; }
        public string Activity { get; set; }
        public string AdditionalExplanation { get; set; }
        public string AllocationCustom6 { get; set; }
        public string BilledCreditCardAccDescr { get; set; }
        public string BilledCreditCardAccNo { get; set; }
        public DateOnly? Date { get; set; }
        public string Department { get; set; }
        public string EmployeeDefCurrAlphaCode { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeLastName { get; set; }
        public DateOnly? EstimatedPaymentDate { get; set; }
        public string JnlPayeePaymentTypeName { get; set; }
        public string JnlPayerPaymentCodeName { get; set; }
        public string JournalAccountCode { get; set; }
        public decimal JournalAmount { get; set; }
        public string JournalDebitOrCredit { get; set; }
        public string PayDemCpnyCashAccCode { get; set; }
        public string PayDemCpnyLBAccCode { get; set; }
        public string Program { get; set; }
        public string ReportEntryDescription { get; set; }
        public string ReportEntryExpenseTypeName { get; set; }
        public string ReportEntryIsPersonalFlag { get; set; }
        public string ReportEntryPaymentCode { get; set; }
        public string ReportEntryPaymentCodeName { get; set; }
        public DateOnly? ReportEntryTransactionDate { get; set; }
        public string ReportEntryVendorDescr { get; set; }
        public string ReportEntryVendorName { get; set; }
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportPaymentReimbType { get; set; }
        public DateOnly? ReportProcessingPaymentDate { get; set; }
        public DateOnly? ReportSubmitDate { get; set; }
        public string accountString { get; set; }
        public string reportPurpose { get; set; }
    }
    public class PostExpensePay
    {
        public PostExpensePay()
        {

        }
        public PostExpensePay(BrexMasterCard brexMasterCard)
        {
            this.Activity = brexMasterCard.Activity;
            this.AdditionalExplanation = brexMasterCard.AdditionalExplanation;
            this.AllocationCustom6 = brexMasterCard.AllocationCustom6;
            this.BilledCreditCardAccDescr = brexMasterCard.BilledCreditCardAccountDescription;
            this.BilledCreditCardAccNo = brexMasterCard.BilledCreditCardAccountNumber;
            this.Date = brexMasterCard.DATE != null ? DateOnly.FromDateTime(brexMasterCard.DATE.Value) : null;
            this.Department = brexMasterCard.Department;
            this.EmployeeDefCurrAlphaCode = brexMasterCard.EmployeeDefaultCurrencyAlphaCode;
            this.EmployeeFirstName = brexMasterCard.EmployeeFirstName;
            this.EmployeeId = brexMasterCard.EmployeeID;
            this.EmployeeLastName = brexMasterCard.EmployeeLastName;
            this.EstimatedPaymentDate = brexMasterCard.EstimatedPaymentDate != null ? DateOnly.FromDateTime(brexMasterCard.EstimatedPaymentDate.Value) : null;
            this.JnlPayeePaymentTypeName = brexMasterCard.JournalPayeePaymentTypeName;
            this.JnlPayerPaymentCodeName = brexMasterCard.JournalPayerPaymentCodeName;
            this.JournalAccountCode = brexMasterCard.JournalAccountCode;
            this.JournalAmount = brexMasterCard.JournalAmount;
            this.JournalDebitOrCredit = brexMasterCard.JournalDebitOrCredit;
            this.PayDemCpnyCashAccCode = brexMasterCard.PaymentDemandCompanyCashAccountCode;
            this.PayDemCpnyLBAccCode = brexMasterCard.PaymentDemandCompanyLiabilityAccountCode;
            this.Program = brexMasterCard.Program;
            this.ReportEntryDescription = brexMasterCard.ReportEntryDescription;
            this.ReportEntryExpenseTypeName = brexMasterCard.ReportEntryExpenseTypeName;
            this.ReportEntryIsPersonalFlag = brexMasterCard.ReportEntryIsPersonalFlag;
            this.ReportEntryPaymentCode = brexMasterCard.ReportEntryPaymentCodeCode;
            this.ReportEntryPaymentCodeName = brexMasterCard.ReportEntryPaymentCodeName;
            this.ReportEntryTransactionDate = brexMasterCard.ReportEntryTransactionDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportEntryTransactionDate.Value) : null;
            this.ReportEntryVendorDescr = brexMasterCard.ReportEntryVendorDescription;
            this.ReportEntryVendorName = brexMasterCard.ReportEntryVendorName;
            this.ReportId = brexMasterCard.ReportID;
            this.ReportName = brexMasterCard.ReportName;
            this.ReportPaymentReimbType = brexMasterCard.ReportPaymentReimbursementType;
            this.ReportProcessingPaymentDate = brexMasterCard.ReportProcessingPaymentDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportProcessingPaymentDate.Value) : null;
            this.ReportSubmitDate = brexMasterCard.ReportSubmitDate != null ? DateOnly.FromDateTime(brexMasterCard.ReportSubmitDate.Value) : null;
            this.accountString = brexMasterCard.AccountString;
        }
        public int EntryNo { get; set; }
        public string Activity { get; set; }
        public string AdditionalExplanation { get; set; }
        public string AllocationCustom6 { get; set; }
        public string BilledCreditCardAccDescr { get; set; }
        public string BilledCreditCardAccNo { get; set; }
        public DateOnly? Date { get; set; }
        public string Department { get; set; }
        public string EmployeeDefCurrAlphaCode { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeLastName { get; set; }
        public DateOnly? EstimatedPaymentDate { get; set; }
        public string JnlPayeePaymentTypeName { get; set; }
        public string JnlPayerPaymentCodeName { get; set; }
        public string JournalAccountCode { get; set; }
        public decimal JournalAmount { get; set; }
        public string JournalDebitOrCredit { get; set; }
        public string PayDemCpnyCashAccCode { get; set; }
        public string PayDemCpnyLBAccCode { get; set; }
        public string Program { get; set; }
        public string ReportEntryDescription { get; set; }
        public string ReportEntryExpenseTypeName { get; set; }
        public string ReportEntryIsPersonalFlag { get; set; }
        public string ReportEntryPaymentCode { get; set; }
        public string ReportEntryPaymentCodeName { get; set; }
        public DateOnly? ReportEntryTransactionDate { get; set; }
        public string ReportEntryVendorDescr { get; set; }
        public string ReportEntryVendorName { get; set; }
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportPaymentReimbType { get; set; }
        public DateOnly? ReportProcessingPaymentDate { get; set; }
        public DateOnly? ReportSubmitDate { get; set; }
        public string accountString { get; set; }
    }
}