using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HYPurchase
{
    [Serializable]
    public class HYEditorProduct
    {
        public string productId;
        public string displayName;
        public string description;
        public long originPrice;
        public float price;
        public string locale;
    }
}

