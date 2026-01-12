using System.Runtime.Serialization;

namespace ImHooya.Purchase.Models
{
    [DataContract]
    public class HyResponseModel
    {
        [DataMember(Name = "responseCode")]
        public int ResponseCode { get; set; }

        [DataMember(Name = "responseMessage")]
        public string ResponseMessage { get; set; } = string.Empty;
    }

    [DataContract]
    public class HyResponseModel<T> : HyResponseModel
    {
        [DataMember(Name = "responseObject")]
        public T ResponseObject { get; set; }
    }

    [DataContract]
    public class HyReceiptModel
    {
        [DataMember(Name = "orderId", EmitDefaultValue = false)]
        public string OrderId { get; set; }

        [DataMember(Name = "purchaseToken", EmitDefaultValue = false)]
        public string PurchaseToken { get; set; }

        [DataMember(Name = "packageName", EmitDefaultValue = false)]
        public string PackageName { get; set; }

        [DataMember(Name = "productId", EmitDefaultValue = false)]
        public string ProductId { get; set; }

        [DataMember(Name = "purchaseTime", EmitDefaultValue = false)]
        public string PurchaseTime { get; set; }

        [DataMember(Name = "paymentId", EmitDefaultValue = false)]
        public string PaymentId { get; set; }

        [DataMember(Name = "purchaseId", EmitDefaultValue = false)]
        public string PurchaseId { get; set; }

        [DataMember(Name = "payload", EmitDefaultValue = false)]
        public string Payload { get; set; }
    }

    public class HyProductModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "priceString")]
        public string PriceString { get; set; }

        [DataMember(Name = "price")]
        public string Price { get; set; }

        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "productId")]
        public string ProductId { get; set; }

        [DataMember(Name = "currencySymbol")]
        public string CurrencySymbol { get; set; }
    }
}