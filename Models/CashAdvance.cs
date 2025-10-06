using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class CashAdvance
    {
        public decimal CashAdvanceRequestAmount { get; set; }
        public string CashAdvanceRequestCurrencyAlphaCode { get; set; }
        public int CashAdvanceRequestCurrencyNumericCode { get; set; }
        public decimal CashAdvanceExchangeRate { get; set; }
        public string CashAdvanceCurrencyAlphaCode { get; set; }
        public int CashAdvanceCurrencyNumericCode { get; set; }
        public DateTime? CashAdvanceIssuedDate { get; set; }
        public string CashAdvancePaymentCodeName { get; set; }
        public int CashAdvanceTransactionType { get; set; }
        public DateTime? CashAdvanceRequestDate { get; set; }
        public int CashAdvanceKey { get; set; }
        public int CashAdvancePaymentMethod { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string JournalPayerPaymentTypeName { get; set; }
        public string JournalPayerPaymentCodeName { get; set; }
        public string JournalPayeePaymentTypeName { get; set; }
        public string JournalPayeePaymentCodeName { get; set; }
        public string JournalAccountCode { get; set; }
        public string JournalDebitOrCredit { get; set; }
        public decimal JournalAmount { get; set; }
        public int BatchID { get; set; }
        public DateTime? BatchDate { get; set; }
    }
    public class PostCashAdvance
    {
        public PostCashAdvance()
        {

        }
        public PostCashAdvance(CashAdvance cashAdvance)
        {
            this.BatchId = cashAdvance.BatchID;
            this.BatchDate = cashAdvance.BatchDate != null ? cashAdvance.BatchDate.Value.ToUniversalTime() : null;
            this.CashAdvCurrAlpCode = cashAdvance.CashAdvanceCurrencyAlphaCode;
            this.CashAdvCurrNumCode = cashAdvance.CashAdvanceCurrencyNumericCode.ToString();
            this.CashAdvReqCurrAlpCode = cashAdvance.CashAdvanceRequestCurrencyAlphaCode;
            this.CashAdvReqCurrNumCode = cashAdvance.CashAdvanceRequestCurrencyNumericCode.ToString();
            this.CashAdvanceExchangeRate = cashAdvance.CashAdvanceExchangeRate;
            this.CashAdvanceIssuedDate = cashAdvance.CashAdvanceIssuedDate != null ? cashAdvance.CashAdvanceIssuedDate.Value.ToUniversalTime() : null;
            this.CashAdvanceKey = cashAdvance.CashAdvanceKey;
            this.CashAdvancePaymentCodeName = cashAdvance.CashAdvancePaymentCodeName;
            this.CashAdvancePaymentMethod = cashAdvance.CashAdvancePaymentMethod;
            this.CashAdvanceRequestAmount = cashAdvance.CashAdvanceRequestAmount;
            this.CashAdvanceRequestDate = cashAdvance.CashAdvanceRequestDate != null ? cashAdvance.CashAdvanceRequestDate.Value.ToUniversalTime() : null;
            this.CashAdvanceTransactionType = cashAdvance.CashAdvanceTransactionType;
            this.EmployeeFirstName = cashAdvance.EmployeeFirstName;
            this.EmployeeID = cashAdvance.EmployeeID;
            this.EmployeeLastName = cashAdvance.EmployeeLastName;
            this.JnlPayeePayCodeName = cashAdvance.JournalPayeePaymentCodeName;
            this.JnlPayeePayTypeName = cashAdvance.JournalPayeePaymentTypeName;
            this.JnlPayerPayCodeName = cashAdvance.JournalPayerPaymentCodeName;
            this.JnlPayerPayTypeName = cashAdvance.JournalPayerPaymentTypeName;
            this.JournalAccountCode = cashAdvance.JournalAccountCode;
            this.JournalAmount = cashAdvance.JournalAmount;
            this.JournalDebitOrCredit = cashAdvance.JournalDebitOrCredit;
        }
        public int BatchId { get; set; }
        public DateTimeOffset? BatchDate { get; set; }
        public string CashAdvCurrAlpCode { get; set; }
        public string CashAdvCurrNumCode { get; set; }
        public string CashAdvReqCurrAlpCode { get; set; }
        public string CashAdvReqCurrNumCode { get; set; }
        public decimal CashAdvanceExchangeRate { get; set; }
        public DateTimeOffset? CashAdvanceIssuedDate { get; set; }
        public int CashAdvanceKey { get; set; }
        public string CashAdvancePaymentCodeName { get; set; }
        public int CashAdvancePaymentMethod { get; set; }
        public decimal CashAdvanceRequestAmount { get; set; }
        public DateTimeOffset? CashAdvanceRequestDate { get; set; }
        public int CashAdvanceTransactionType { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeLastName { get; set; }
        public string JnlPayeePayCodeName { get; set; }
        public string JnlPayeePayTypeName { get; set; }
        public string JnlPayerPayCodeName { get; set; }
        public string JnlPayerPayTypeName { get; set; }
        public string JournalAccountCode { get; set; }
        public decimal JournalAmount { get; set; }
        public string JournalDebitOrCredit { get; set; }
    }

}