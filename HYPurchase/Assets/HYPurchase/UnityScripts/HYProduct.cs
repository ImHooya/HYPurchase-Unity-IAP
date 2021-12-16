using Newtonsoft.Json;

namespace HYPurchase
{
    /// <summary>
    /// Products from platform stores
    /// </summary>
    [System.Serializable]
    public class HYProduct
    {
        /// <summary>
        /// Product ID
        /// </summary>
        [JsonProperty]
        public readonly string productId;
        /// <summary>
        /// Product's display name
        /// </summary>
        [JsonProperty]
        public readonly string displayName;
        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty]
        public readonly string description;
        /// <summary>
        /// Price from platform stores(Android PlayStore = "€7.99" is "7990000", Steam = "€7.99" is "799")
        /// </summary>
        [JsonProperty]
        public readonly long originPrice;
        /// <summary>
        /// Display price
        /// </summary>
        [JsonProperty]
        public readonly string price;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        public readonly string locale;
    }
}

