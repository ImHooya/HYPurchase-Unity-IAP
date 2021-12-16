using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace HYPurchase
{
    public class SteamPurchase : IHYPlatformPurchase
    {
        private const string DYNAMIC_PROPS_STRING = "dynamic_props";
        private const string RECEIPT_STRING = "receipt";
        private class SteamProduct
        {
            public SteamItemDef_t itemDefinition { get; }
            public string productId { get; }

            public SteamProduct(SteamItemDef_t definition, string productId)
            {
                this.itemDefinition = definition;
                this.productId = productId;
            }
        }

        private class SteamInventoryItem
        {
            public SteamItemDetails_t Detail { get; set; }
            public SteamItemInstanceID_t ItemId => Detail.m_itemId;
            public SteamItemDef_t Definition => Detail.m_iDefinition;

            public SteamInventoryItem(SteamItemDetails_t detail)
            {
                this.Detail = detail;
            }
        }

        private string currencyCode;
        private GameObject mainThreadCallback = null;
        private List<SteamProduct> steamProductList = new List<SteamProduct>();
        private IDisposable steamInventoryResultReadyDisposable;
        private SteamInventoryItem lastUpdatedItem = null;
        private List<SteamInventoryItem> inventoryItemList = new List<SteamInventoryItem>();
        private Action<SteamInventoryItem> steamInventoryIteamCallResult = null;
        private Action<SteamInventoryResult_t> steamInventoryFailedResultCallResult = null;
        internal SteamPurchase()
        {
            SteamAPI.Init();
            if (mainThreadCallback == null)
            {
                mainThreadCallback = new GameObject();
                mainThreadCallback.AddComponent<MainThreadCallback>();
            }
        }

        public void Init(Action<OnConnectComplete> onConnectComplete)
        {
            SteamInventoryResult_t handle;

            Callback<SteamInventoryFullUpdate_t>.Create(callback =>
            {
                bool result = false;
                SteamItemDetails_t[] detailArray = null;
                uint count = 0;
                if (SteamInventory.GetResultItems(callback.m_handle, null, ref count))
                {
                    detailArray = new SteamItemDetails_t[count];
                    result = SteamInventory.GetResultItems(callback.m_handle, detailArray, ref count);

                    for (int i = 0; i < count; i++)
                    {
                        SteamInventoryItem item = new SteamInventoryItem(detailArray[i]);
                        inventoryItemList.Add(item);
                    }
                }
                SteamInventory.DestroyResult(callback.m_handle);
                List<string> unConsumeList = new List<string>();
                inventoryItemList.ForEach(x =>
                {
                    unConsumeList.Add(GetSteamItemProperty(x.Definition, "productId"));
                });
                onConnectComplete(new OnConnectComplete(result ? 0 : 1, "finished", unConsumeList));
            });

            SteamInventory.GetAllItems(out handle);

            steamInventoryResultReadyDisposable = Callback<SteamInventoryResultReady_t>.Create(callback =>
            {
                bool result = false;
                SteamItemDetails_t[] detailArray = null;
                uint count = 0;

                if (callback.m_result == EResult.k_EResultOK)
                {
                    bool ret = SteamInventory.GetResultItems(callback.m_handle, null, ref count);
                    if (ret)
                    {
                        detailArray = new SteamItemDetails_t[count];
                        result = SteamInventory.GetResultItems(callback.m_handle, detailArray, ref count);
                        for (int i = 0; i < count; i++)
                        {
                            SteamInventoryItem item = new SteamInventoryItem(detailArray[i]);

                            if (steamInventoryIteamCallResult != null)
                                steamInventoryIteamCallResult(item);

                            if (!inventoryItemList.Any(inventoryItem => inventoryItem.ItemId == item.ItemId))
                                inventoryItemList.Add(item);
                        }
                    }
                }
                else
                {
                    if (steamInventoryFailedResultCallResult != null)
                        steamInventoryFailedResultCallResult(callback.m_handle);
                }
            });
        }

        public void Purchase(string productId, string developerPayload, Action<OnPurchaseComplete> onPurchaseComplete)
        {
            var purchaseProduct = steamProductList.First(product => product.productId == productId);

            var handle = SteamInventory.StartPurchase(new SteamItemDef_t[] { purchaseProduct.itemDefinition }, new uint[] { 1 }, 1);
            int retryCount = 0;
            var startPurchaseResult = CallResult<SteamInventoryStartPurchaseResult_t>.Create((result, bIOFailure) =>
            {
                Callback<MicroTxnAuthorizationResponse_t>.Create(callback =>
                {
                    if (callback.m_bAuthorized == 0)
                    {
                        steamInventoryIteamCallResult = null;
                        steamInventoryFailedResultCallResult = null;
                        onPurchaseComplete(new OnPurchaseComplete(1, "purchase cancelled", null));
                    }
                });

                JObject jobj = new JObject();
                jobj.Add("txnId", new JValue(result.m_ulTransID));
                jobj.Add("orderId", new JValue(result.m_ulOrderID));
                jobj.Add("payload", new JValue(developerPayload));

                steamInventoryIteamCallResult = (item) =>
                {
                    if (lastUpdatedItem != null && item.ItemId == lastUpdatedItem.ItemId)
                    {
                        JObject receipt = new JObject();
                        receipt.Add("productId", productId);
                        receipt.Add("receipt", jobj.ToString());
                        receipt.Add("developerPayload", developerPayload);

                        onPurchaseComplete(new OnPurchaseComplete(0, "success", receipt.ToObject<HYReceipt>()));
                        steamInventoryIteamCallResult = null;
                        steamInventoryFailedResultCallResult = null;
                        return;
                    }

                    lastUpdatedItem = item;

                    SteamInventoryUpdateHandle_t updateHandle = SteamInventory.StartUpdateProperties();
                    SteamInventoryResult_t resultHandle;

                    SteamInventory.SetProperty(updateHandle, item.ItemId, RECEIPT_STRING, jobj.ToString());
                    SteamInventory.SubmitUpdateProperties(updateHandle, out resultHandle);
                };

                steamInventoryFailedResultCallResult = (handle) =>
                {
                    if (retryCount > 2)
                        return;

                    retryCount++;

                    SteamInventoryUpdateHandle_t updateHandle = SteamInventory.StartUpdateProperties();
                    SteamInventoryResult_t resultHandle;
                    SteamInventory.SetProperty(updateHandle, lastUpdatedItem.ItemId, RECEIPT_STRING, jobj.ToString());
                    SteamInventory.SubmitUpdateProperties(updateHandle, out resultHandle);
                };
            });

            startPurchaseResult.Set(handle);
        }

        public void Refresh(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete)
        {
            var handle = SteamInventory.RequestPrices();

            var callResult = CallResult<SteamInventoryRequestPricesResult_t>.Create((response, fail) =>
            {
                if (response.m_result != EResult.k_EResultOK)
                {
                    onRefreshComplete(new OnRefreshComplete((int)response.m_result, response.m_result.ToString(), null));
                    return;
                }
                currencyCode = GetLocale(response.m_rgchCurrency);
                UpdateInventoryProducts(productIdList, onRefreshComplete);
            });
            callResult.Set(handle);
        }

        private void UpdateInventoryProducts(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete)
        {
            uint num = SteamInventory.GetNumItemsWithPrices();
            SteamItemDef_t[] buffer = new SteamItemDef_t[num];
            ulong[] basePrices = new ulong[num];
            ulong[] currentPrices = new ulong[num];
            List<HYProduct> productList = new List<HYProduct>();
            if (SteamInventory.GetItemsWithPrices(buffer, currentPrices, basePrices, num))
            {
                for (int i = 0; i < num; i++)
                {
                    JObject jobj = new JObject();
                    SteamItemDef_t itemDef = buffer[i];

                    string productId = GetSteamItemProperty(itemDef, "productId");

                    if (!productIdList.Contains(productId))
                        continue;

                    jobj.Add("productId", productId);
                    jobj.Add("displayName", new JValue(GetSteamItemProperty(itemDef, "name")));
                    jobj.Add("description", new JValue(GetSteamItemProperty(itemDef, "description")));
                    jobj.Add("originPrice", new JValue(currentPrices[i].ToString()));
                    jobj.Add("price", new JValue((currentPrices[i] * 0.01f).ToString()));
                    jobj.Add("locale", currencyCode);
                    HYProduct product = jobj.ToObject<HYProduct>();
                    SteamProduct steamProduct = new SteamProduct(itemDef, productId);
                    steamProductList.Add(steamProduct);
                    productList.Add(product);
                }
            }
            onRefreshComplete(new OnRefreshComplete(0, "success", productList));
        }

        private string GetSteamItemProperty(SteamItemDef_t def, string property)
        {
            uint size = 0;
            string buffer = null;
            if (SteamInventory.GetItemDefinitionProperty(def, property, out buffer, ref size))
            {
                if (SteamInventory.GetItemDefinitionProperty(def, property, out buffer, ref size))
                {
                    return buffer;
                }
            }

            return "";
        }

        public void Consume(string productId, Action<OnConsumeComplete> onConsumeComplete)
        {
            SteamInventoryItem item = inventoryItemList.FirstOrDefault(item => GetSteamItemProperty(item.Definition, "productId") == productId);

            if (item == null || item.ItemId == null)
            {
                onConsumeComplete(new OnConsumeComplete(1, "no consumable item"));
                return;
            }

            SteamInventoryResult_t resultHandle;
            SteamInventory.ConsumeItem(out resultHandle, item.ItemId, 1);

            steamInventoryIteamCallResult = (item) =>
            {
                if ((item.Detail.m_unFlags & (int)ESteamItemFlags.k_ESteamItemRemoved) > 0 || (item.Detail.m_unFlags & (int)ESteamItemFlags.k_ESteamItemConsumed) > 0)
                {
                    onConsumeComplete(new OnConsumeComplete(0, "success"));
                    steamInventoryIteamCallResult = null;
                    return;
                }
            };
        }

        public void Restore(string productId, Action<OnRestoreComplete> onRestoreComplete)
        {
            SteamInventoryItem item = inventoryItemList.First(item => GetSteamItemProperty(item.Definition, "productId") == productId);

            if (item == null)
            {
                onRestoreComplete(new OnRestoreComplete(1, "no restore item", null));
                return;
            }

            SteamInventoryResult_t handle;
            if (SteamInventory.GetItemsByID(out handle, new SteamItemInstanceID_t[1] { item.ItemId }, 1))
            {
                string valueBuffer;
                uint valueBufferSize = 0;

                bool ret = SteamInventory.GetResultItemProperty(handle, (uint)0, DYNAMIC_PROPS_STRING, out valueBuffer, ref valueBufferSize);

                if (ret)
                {
                    ret = SteamInventory.GetResultItemProperty(handle, (uint)0, DYNAMIC_PROPS_STRING, out valueBuffer, ref valueBufferSize);

                    JObject jobj = new JObject(valueBuffer);

                    onRestoreComplete(new OnRestoreComplete(0, "success", jobj.ToObject<HYReceipt>()));
                    return;
                }
            }

            onRestoreComplete(new OnRestoreComplete(999, "FAILED", null));
        }

        private string GetLocale(string isoCurrencySymbol)
        {
            string locale = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => !culture.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == isoCurrencySymbol)
                .Select(ri => ri.Name)
                .FirstOrDefault();

            return locale;
        }

        public void Dispose()
        {
            GameObject.Destroy(mainThreadCallback);
        }
    }
}
