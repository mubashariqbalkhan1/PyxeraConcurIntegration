
using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class ExpenseCashAdvance
    {
        public ExpenseCashAdvance() { }
        public ExpenseCashAdvance(ExpenseCashAdvance item)
        {
            PaymentType = item.PaymentType;
            ExchangeRate = item.ExchangeRate;
            AmountRequested = item.AmountRequested;
            AvailableBalance = item.AvailableBalance;
            ApprovalStatus = item.ApprovalStatus;
            CashAdvanceId = item.CashAdvanceId;
            RequestDate = item.RequestDate;
            Name = item.Name;
            Purpose = item.Purpose;
            IssuedDate = item.IssuedDate;
            AccountCode = item.AccountCode;
            Comment = item.Comment;
            LastModifiedDate = item.LastModifiedDate;
            HasReceipts = item.HasReceipts;
            ReimbursementCurrency = item.ReimbursementCurrency;
        }
        [JsonPropertyName("paymentType")]
        public PaymentType PaymentType { get; set; }

        [JsonPropertyName("exchangeRate")]
        public ExchangeRate ExchangeRate { get; set; }

        [JsonPropertyName("amountRequested")]
        public Money AmountRequested { get; set; }

        [JsonPropertyName("availableBalance")]
        public Money AvailableBalance { get; set; }

        [JsonPropertyName("approvalStatus")]
        public ApprovalStatus ApprovalStatus { get; set; }

        [JsonPropertyName("cashAdvanceId")]
        public string CashAdvanceId { get; set; }

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        [JsonPropertyName("issuedDate")]
        public DateTime IssuedDate { get; set; }

        [JsonPropertyName("accountCode")]
        public string AccountCode { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonPropertyName("hasReceipts")]
        public bool HasReceipts { get; set; }

        [JsonPropertyName("reimbursementCurrency")]
        public string ReimbursementCurrency { get; set; }
        public string ReportId { get; set; }
    }

    public class PaymentType
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("paymentCode")]
        public string PaymentCode { get; set; }
    }

    public class ExchangeRate
    {
        [JsonPropertyName("operation")]
        public string Operation { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }
    }

    public class Money
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public class ApprovalStatus
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class CashAdvanceResponse
    {
        [JsonPropertyName("cashAdvances")]
        public List<CashAdvanceItem> CashAdvances { get; set; }
    }

    public class CashAdvanceItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class BC_ExpenseCashAdvance
    {
        public BC_ExpenseCashAdvance() { }
        public BC_ExpenseCashAdvance(ExpenseCashAdvance item)
        {
            CashAdvanceId = item.CashAdvanceId;
            ReportId = item.ReportId;
            AccountCode = item.AccountCode;
            Amount = item.AmountRequested != null ? item.AmountRequested.Amount : 0;
            ApprovalCode = item.ApprovalStatus != null ? item.ApprovalStatus.Code : null;
            ApprovalStatus = item.ApprovalStatus != null ? item.ApprovalStatus.Name : null;
            AvailableBalance = item.AvailableBalance != null ? item.AvailableBalance.Amount : 0;
            Currency = item.AmountRequested != null ? item.AmountRequested.Currency : null;
            ExchangeRate = item.ExchangeRate != null ? item.ExchangeRate.Value : 0;
            ExchangeRateOp = item.ExchangeRate != null ? item.ExchangeRate.Operation : null;
            hasReciept = item.HasReceipts;
            IssuedDate = item.IssuedDate != DateTime.MinValue ? item.IssuedDate : (DateTimeOffset?)null;
            Name = item.Name;
            PaymentMethod = item.PaymentType != null ? item.PaymentType.PaymentCode : null;
            PaymentTypeDesc = item.PaymentType != null ? item.PaymentType.Description : null;
            ReimbursementCurrency = item.ReimbursementCurrency;
            RequestDate = item.RequestDate != DateTime.MinValue ? item.RequestDate : (DateTimeOffset?)null;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        [JsonPropertyName("cashAdvanceId")]
        public string CashAdvanceId { get; set; }

        [JsonPropertyName("reportId")]
        public string ReportId { get; set; }

        [JsonPropertyName("accountCode")]
        public string AccountCode { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("approvalCode")]
        public string ApprovalCode { get; set; }

        [JsonPropertyName("approvalStatus")]
        public string ApprovalStatus { get; set; }

        [JsonPropertyName("availableBalance")]
        public decimal AvailableBalance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("exchangeRate")]
        public decimal ExchangeRate { get; set; }

        [JsonPropertyName("exchangeRateOp")]
        public string ExchangeRateOp { get; set; }

        [JsonPropertyName("hasReciept")]
        public bool hasReciept { get; set; }

        [JsonPropertyName("issuedDate")]
        public DateTimeOffset? IssuedDate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("paymentTypeDesc")]
        public string PaymentTypeDesc { get; set; }

        [JsonPropertyName("reimbursementCurrency")]
        public string ReimbursementCurrency { get; set; }

        [JsonPropertyName("requestDate")]
        public DateTimeOffset? RequestDate { get; set; }
    }

}