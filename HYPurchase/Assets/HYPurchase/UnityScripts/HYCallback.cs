using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HYPurchase
{
    public class OnComplete
    {
        public bool isSuccess => responseCode == 0;
        public string message;
        public int responseCode;

        public OnComplete(int responseCode, string message)
        {
            this.responseCode = responseCode;
            this.message = message;
        }
    }
    public class OnConnectComplete : OnComplete
    {
        public List<string> unConsumedProductIdList;
        public OnConnectComplete(int responseCode, string message, List<string> unConsumedProductIdList) : base(responseCode, message)
        {
            this.unConsumedProductIdList = unConsumedProductIdList;
        }
    }
    public class OnRefreshComplete : OnComplete
    {
        public List<HYProduct> productList;
        public OnRefreshComplete(int responseCode, string message, List<HYProduct> productList) : base(responseCode, message)
        {
            this.productList = productList;
        }
    }
    public class OnGetUnConsumedProductsComplete : OnComplete
    {
        public List<HYReceipt> unConsumedProductList;
        public OnGetUnConsumedProductsComplete(int responseCode, string message, List<HYReceipt> productList) : base(responseCode, message)
        {
            this.unConsumedProductList = productList;
        }
    }
    public class OnPurchaseComplete : OnComplete
    {
        public HYReceipt receipt;
        public OnPurchaseComplete(int responseCode, string message, HYReceipt receipt) : base(responseCode, message)
        {
            this.receipt = receipt;
        }
    }
    public class OnConsumeComplete : OnComplete
    {
        public OnConsumeComplete(int responseCode, string message) : base(responseCode, message)
        {

        }
    }
    public class OnRestoreComplete : OnComplete
    {
        public HYReceipt receipt;
        public OnRestoreComplete(int responseCode, string message, HYReceipt receipt) : base(responseCode, message)
        {
            this.receipt = receipt;
        }
    }
}

