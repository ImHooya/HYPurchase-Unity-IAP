using System;
using System.Collections.Generic;
using ImHooya.Purchase.Models;
using UnityEngine;

namespace ImHooya.Purchase
{
    public interface IHYStorePurchase
    {
        public PurchaseStore GetStoreType();
        public void Init(Action<HyResponseModel> callback);
        public void RefreshProducts(List<string> productIds, Action<HyResponseModel<List<HyProductModel>>> callback);
        public void GetUnconsumedReceipts(Action<HyResponseModel<List<HyReceiptModel>>> callback);
        public void Purchase(string itemId, string payload, Action<HyResponseModel<HyReceiptModel>> callback);
        public void Consume(string itemId, Action<HyResponseModel> callback);
        public void ConsumeAll(Action<HyResponseModel> callback);
    }
}
