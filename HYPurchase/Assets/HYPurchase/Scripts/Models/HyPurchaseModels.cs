using System.Runtime.Serialization;

namespace ImHooya.Purchase.Models
{
    [DataContract]
    public class HyResponseModel
    {
        [DataMember(Name = "responseCode")]
        public int ResponseCode { get; internal set; }

        [DataMember(Name = "responseMessage")]
        public string ResponseMessage { get; internal set; } = string.Empty;
    }

    [DataContract]
    public class HyResponseModel<T> : HyResponseModel
    {
        [DataMember(Name = "responseObject")]
        public T ResponseObject { get; internal set; }
    }

    [DataContract]
    public class HyReceiptModel
    {
        [DataMember(Name = "orderId", EmitDefaultValue = false)]
        public string OrderId { get; internal set; }

        [DataMember(Name = "purchaseToken", EmitDefaultValue = false)]
        public string PurchaseToken { get; internal set; }

        [DataMember(Name = "packageName", EmitDefaultValue = false)]
        public string PackageName { get; internal set; }

        [DataMember(Name = "productId", EmitDefaultValue = false)]
        public string ProductId { get; internal set; }

        [DataMember(Name = "purchaseTime", EmitDefaultValue = false)]
        public string PurchaseTime { get; internal set; }

        [DataMember(Name = "paymentId", EmitDefaultValue = false)]
        public string PaymentId { get; internal set; }

        [DataMember(Name = "purchaseId", EmitDefaultValue = false)]
        public string PurchaseId { get; internal set; }

        [DataMember(Name = "payload", EmitDefaultValue = false)]
        public string Payload { get; internal set; }

        [DataMember(Name = "productType")]
        public string ProductType { get; internal set; }
    }

    [DataContract]
    public class HyProductModel
    {
        [DataMember(Name = "title")]
        public string Title { get; internal set; }

        [DataMember(Name = "priceString")]
        public string PriceString { get; internal set; }

        [DataMember(Name = "price")]
        public string Price { get; internal set; }

        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; internal set; }

        [DataMember(Name = "description")]
        public string Description { get; internal set; }

        [DataMember(Name = "productId")]
        public string ProductId { get; internal set; }

        [DataMember(Name = "currencySymbol")]
        public string CurrencySymbol { get; internal set; }

        [DataMember(Name = "productType")]
        public string ProductType { get; internal set; }
    }

    [DataContract]
    public class HyStoreCountryModel
    {
        [DataMember(Name = "countryCode")]
        public string CountryCode { get; internal set; }
    }
}
