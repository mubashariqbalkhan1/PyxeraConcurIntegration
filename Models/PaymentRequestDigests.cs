using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class PaymentRequestDigestsRoot
    {
        public PaymentRequestDigestsList? PaymentRequestDigests { get; set; }
    }
    public class PaymentRequestDigestsList
    {
        [JsonProperty("@xmlns:xsd")]
        public string? xmlnsxsd { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string? xmlnsxsi { get; set; }
        public List<PaymentRequestDigest>? PaymentRequestDigest { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? NextPage { get; set; }
    }
    public class PaymentRequestDigest
    {
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ID { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? URI { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentRequestId { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentRequestUri { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ApprovalStatusCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentStatusCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VendorCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VendorName { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? InvoiceNumber { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? CreateDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? UserDefinedDate { get; set; }

        public decimal Total { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? CurrencyCode { get; set; }

        public bool IsDeleted { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentMethod { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? LastModifiedDate { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PurchaseOrderNumber { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OwnerLoginID { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OwnerName { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? ExtractedDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? PaidDate { get; set; }
    }
    public class PaymentRequestRoot
    {
        public PaymentRequest? PaymentRequest { get; set; }
    }
    public class LineItemsWrapper
    {
        [JsonProperty("LineItem")]
        [JsonConverter(typeof(SingleOrArrayConverter<LineItem>))]
        public List<LineItem>? LineItem { get; set; }
    }
    public class PaymentRequest
    {
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ID { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? URI { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? EmployeeName { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? CurrencyCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? InvoiceNumber { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? DataSource { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? LedgerCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? CountryCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Name { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? OB10TransactionId { get; set; }

        public int PaymentTermsDays { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentMethod { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ReceiptConfirmationType { get; set; }

        public bool IsInvoiceConfirmed { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ApprovalStatus { get; set; }
        public bool SubmittedByDelegate { get; set; }
        public bool ApprovedByDelegate { get; set; }
        public bool IsAssigned { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? CreatedByUsername { get; set; }

        public bool PaymentRequestCreatedByTestUser { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentRequestDeletedBy { get; set; }

        public bool IsPaymentRequestDeleted { get; set; }
        public bool IsTestTransaction { get; set; }
        public bool IsPaymentRequestDuplicate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? UserCreationDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? InvoiceDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? PaymentDueDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? DeletedDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? InvoiceReceivedDate { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? PaidDate { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? ExtractDate { get; set; }
        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? FirstApprovalDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? FirstSubmitDate { get; set; }

        [JsonConverter(typeof(NullReplacementDateConverter))]
        public DateTimeOffset? LastSubmitDate { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PaymentStatus { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Description { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? NotesToVendor { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PoNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal CalculatedAmount { get; set; }
        public decimal TotalApprovedAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LineItemTotalAmount { get; set; }
        public decimal LineItemVatAmount { get; set; }
        public decimal AmountWithoutVat { get; set; }
        public decimal VatAmountOne { get; set; }
        public decimal VatAmountTwo { get; set; }
        public decimal VatAmountThree { get; set; }
        public decimal VatAmountFour { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VatRateOne { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VatRateTwo { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VatRateThree { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VatRateFour { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ProvincialTaxId { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VendorTaxId { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ExternalPolicyId { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode2 { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode3 { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode4 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom3 { get; set; }

        public VendorRemitAddress? VendorRemitAddress { get; set; }
        public LineItemsWrapper? LineItems { get; set; }

    }
    public class LineItemEx
    {
        public List<LineItem>? LineItem { get; set; }
    }

    public class VendorRemitAddress
    {
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Address1 { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Address2 { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? City { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? State { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? PostalCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? CountryCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Name { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? AddressCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? VendorCode { get; set; }
    }


    public class LineItem
    {
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? LineItemId { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? ExpenseTypeCode { get; set; }

        public int RequestLineItemNumber { get; set; }
        public decimal Quantity { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Description { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? AllocationStatus { get; set; }

        public bool IsMatched { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? SupplierPartId { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal RequestedLineItemAmount { get; set; }
        public decimal ApprovedLineItemAmount { get; set; }
        public decimal AmountWithoutVat { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatAmountTwo { get; set; }
        public decimal VatAmountThree { get; set; }
        public decimal VatAmountFour { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode2 { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode3 { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? TaxCode4 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom1 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom2 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom3 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom4 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom5 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom6 { get; set; }
        public AllocationsWrapper? Allocations { get; set; }
        public object? MatchedPurchaseOrderReceipts { get; set; }
    }
    public class AllocationsWrapper
    {
        [JsonProperty("Allocation")]
        [JsonConverter(typeof(SingleOrArrayConverter<Allocation>))]
        public List<Allocation>? Allocation { get; set; }
    }
    public class Allocation
    {
        public decimal Percentage { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? AllocationAccountCode { get; set; }

        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom1 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom2 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom3 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom4 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom5 { get; set; }
        [JsonConverter(typeof(NullReplacementConverter))]
        public string? Custom6 { get; set; }
    }

    #region Business Central
    public class BC_InvoiceHeader
    {
        public BC_InvoiceHeader()
        {

        }
        public BC_InvoiceHeader(PaymentRequest pr)
        {
            amountWithoutVAT = pr.AmountWithoutVat;
            approvalStatus = pr.ApprovalStatus;
            assigned = pr.IsAssigned;
            city = pr.VendorRemitAddress?.City;
            countryCode = pr.CountryCode;
            createdByUser = pr.CreatedByUsername;
            currencyCode = pr.CurrencyCode;
            description = pr.Description;
            employeeName = pr.EmployeeName;
            extractDate = pr.ExtractDate?.ToUniversalTime();
            firstApprovalDate = pr.FirstApprovalDate?.ToUniversalTime();
            firstSubmitDate = pr.FirstSubmitDate?.ToUniversalTime();
            id = pr.ID;
            invoiceAmount = pr.InvoiceAmount;
            invoiceConfirmed = pr.IsInvoiceConfirmed;
            invoiceDate = pr.InvoiceDate?.ToUniversalTime();
            invoiceNumber = pr.InvoiceNumber;
            lastSubmitDate = pr.LastSubmitDate?.ToUniversalTime();
            ledgerCode = pr.LedgerCode;
            lineTotalAmount = pr.LineItemTotalAmount;
            lineVATAmount = pr.LineItemVatAmount;
            name = pr.Name;
            notesToVendor = pr.NotesToVendor;
            poNumber = !string.IsNullOrEmpty(pr.PoNumber) ? pr.PoNumber : "";
            paidAmount = pr.PaidAmount;
            paidDate = pr.PaidDate?.ToUniversalTime();
            paymentDueDate = pr.PaymentDueDate?.ToUniversalTime();
            paymentMethod = pr.PaymentMethod;
            paymentStatus = pr.PaymentStatus;
            paymentTermsDays = pr.PaymentTermsDays;
            postalCode = pr.VendorRemitAddress?.PostalCode;
            state = pr.VendorRemitAddress?.State;
            totalApprovedAmount = pr.TotalApprovedAmount;
            userCreationDate = pr.UserCreationDate?.ToUniversalTime();
            vendCountryCode = pr.VendorRemitAddress?.CountryCode;
            vendorCode = pr.VendorRemitAddress?.VendorCode;
            vendorName = pr.VendorRemitAddress?.Name;
            vendorRemitAdd = pr.VendorRemitAddress.Address1;
            vendorRemitAdd2 = pr.VendorRemitAddress.Address2;
            postingPeriod = pr.Custom3;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public decimal amountWithoutVAT { get; set; }
        public string approvalStatus { get; set; }
        public bool assigned { get; set; }
        public string? city { get; set; }
        public string? countryCode { get; set; }
        public string? createdByUser { get; set; }
        public string? currencyCode { get; set; }
        public string? description { get; set; }
        public string? employeeName { get; set; }
        public DateTimeOffset? extractDate { get; set; }
        public DateTimeOffset? firstApprovalDate { get; set; }
        public DateTimeOffset? firstSubmitDate { get; set; }
        public string? id { get; set; }
        public decimal invoiceAmount { get; set; }
        public bool invoiceConfirmed { get; set; }
        public DateTimeOffset? invoiceDate { get; set; }
        public string invoiceNumber { get; set; }
        public DateTimeOffset? lastSubmitDate { get; set; }
        public string? ledgerCode { get; set; }
        public decimal lineTotalAmount { get; set; }
        public decimal lineVATAmount { get; set; }
        public string? name { get; set; }
        public string? notesToVendor { get; set; }
        public string? poNumber { get; set; }
        public decimal paidAmount { get; set; }
        public DateTimeOffset? paidDate { get; set; }
        public DateTimeOffset? paymentDueDate { get; set; }
        public string? paymentMethod { get; set; }
        public string? paymentStatus { get; set; }
        public int paymentTermsDays { get; set; }
        public string? postalCode { get; set; }
        public string? state { get; set; }
        public decimal totalApprovedAmount { get; set; }
        public DateTimeOffset? userCreationDate { get; set; }
        public string? vendCountryCode { get; set; }
        public string? vendorCode { get; set; }
        public string? vendorName { get; set; }
        public string? vendorRemitAdd { get; set; }
        public string? vendorRemitAdd2 { get; set; }
        public string? postingPeriod { get; set; }
    }
    public class BC_InvoiceLineItem
    {
        public BC_InvoiceLineItem()
        {
        }
        public BC_InvoiceLineItem(LineItem li, string headerId)
        {
            activityCode = li.Custom5;
            allocationStatus = li.AllocationStatus;
            amtWithoutVAT = li.AmountWithoutVat;
            appLineItemAmt = li.ApprovedLineItemAmount;
            departmentCode = li.Custom2;
            detailedDescription = li.Custom1;
            expenseTypeCode = li.ExpenseTypeCode;
            lineItemId = li.LineItemId;
            locationCode = li.Custom6;
            paymentRequestId = headerId;
            programCode = li.Custom4;
            quantity = li.Quantity;
            reqLineItemAmt = li.RequestedLineItemAmount;
            requestItemNo = li.RequestLineItemNumber;
            totalPrice = li.TotalPrice;
            unitPrice = li.UnitPrice;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public string? activityCode { get; set; }
        public string? allocationStatus { get; set; }
        public decimal amtWithoutVAT { get; set; }
        public decimal appLineItemAmt { get; set; }
        public string? departmentCode { get; set; }
        public string? detailedDescription { get; set; }
        public string? expenseTypeCode { get; set; }
        public string? lineItemId { get; set; }
        public string? locationCode { get; set; }
        public string? paymentRequestId { get; set; }
        public string? programCode { get; set; }
        public decimal quantity { get; set; }
        public decimal reqLineItemAmt { get; set; }
        public int requestItemNo { get; set; }
        public decimal totalPrice { get; set; }
        public decimal unitPrice { get; set; }
    }
    public class BC_Invoice_Allocations
    {
        public BC_Invoice_Allocations()
        {

        }
        public BC_Invoice_Allocations(Allocation ac, string lineItemId, string requestId)
        {
            activityCode = ac.Custom5;
            allocAccCode = ac.AllocationAccountCode;
            allocationPercentage = ac.Percentage;
            departmentCode = ac.Custom2;
            detaliedDescription = ac.Custom1;
            lineItemReference = lineItemId;
            locationCode = ac.Custom6;
            paymentRequestId = requestId;
            programCode = ac.Custom4;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemId { get; set; }
        public string? activityCode { get; set; }
        public string? allocAccCode { get; set; }
        public decimal? allocationPercentage { get; set; }
        public string? departmentCode { get; set; }
        public string? detaliedDescription { get; set; }
        public string? lineItemReference { get; set; }
        public string? locationCode { get; set; }
        public string? paymentRequestId { get; set; }
        public string? programCode { get; set; }
    }
    #endregion

}


