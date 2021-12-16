using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HYPurchase
{
    public class HYPurchaseEditor : ScriptableObject
    {
        [MenuItem("HYPurchase/Editor Store Setting")]
        static void ShowHYPurchaseEditor()
        {
            Selection.activeObject = GetScriptableObject();
        }

        public static HYPurchaseEditor GetInstance()
        {
            return GetScriptableObject();
        }

        private static HYPurchaseEditor GetScriptableObject()
        {
            string path = "";
            string[] pathArray = AssetDatabase.FindAssets("EditorPurchaseSetting");

            if (pathArray == null || pathArray.Length == 0)
            {
                ScriptableObject.CreateInstance("HYPurchaseEditor");
            }

            path = AssetDatabase.FindAssets("EditorPurchaseSetting")[0];
            path = AssetDatabase.GUIDToAssetPath(path);
            return AssetDatabase.LoadAssetAtPath<HYPurchaseEditor>(path);
        }
        public bool ConnectSuccess = true;
        public bool RefreshSuccess = true;
        public bool PurchaseSuccess = true;
        public bool ConsumeSuccess = true;
        public bool RestoreSuccess = true;

        public bool AutoConsume = false;

        public List<HYEditorProduct> productList = new List<HYEditorProduct>();
    }
}

