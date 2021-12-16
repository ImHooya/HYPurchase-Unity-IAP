using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace HYPurchase
{
    public class HYReceipt
    {
        [JsonProperty]
        public readonly string productId;
        [JsonProperty]
        public readonly string receipt;
        [JsonProperty]
        public readonly string orderId;
        [JsonProperty]
        public readonly string developerPayload;
    }
}
