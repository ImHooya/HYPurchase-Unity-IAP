# HYPurchase Unity IAP

HYPurchase is a Unity wrapper for Google Play Billing on Android and StoreKit 2 on iOS.

## Supported Platforms

| Platform | Native implementation |
| --- | --- |
| Android | Google Play Billing Client 9.1.0 |
| iOS | StoreKit 2 |

Use the native library built for the same HYPurchase Unity package revision. The Android AAR must include the current `IHyPurchaseHandler` callbacks, including `consumeAllComplete` and `acknowledgeComplete`.

## Android Gradle Setup

The Android native library uses Google Play Billing Client `9.1.0`.

In Unity, enable **Custom Main Gradle Template** under **Project Settings > Player > Publishing Settings**. Then ensure `Assets/Plugins/Android/mainTemplate.gradle` includes the following dependencies inside its `dependencies` block:

```gradle
dependencies {
    implementation fileTree(dir: 'libs', include: ['*.jar'])

    implementation 'com.android.billingclient:billing:9.1.0'
    implementation 'org.jetbrains.kotlinx:kotlinx-serialization-json:1.6.3'
**DEPS**}
```

The included `mainTemplate.gradle` already contains these entries. Keep a single Billing Client dependency and use version `9.1.0`; do not add another Billing Client version through a different Gradle template or dependency resolver. The template targets Java 17, so Unity's Android JDK must also be Java 17 or newer.

## Initialization

Initialize once before calling another API.

```csharp
using System.Collections.Generic;
using ImHooya.Purchase;
using ImHooya.Purchase.Models;
using UnityEngine;

public class PurchaseExample : MonoBehaviour
{
    private void Start()
    {
#if UNITY_IOS
        var store = PurchaseStore.AppStore;
#else
        var store = PurchaseStore.PlayStore;
#endif

        HYPurchase.Init(store, response =>
        {
            Debug.Log($"Init: {response.ResponseCode} - {response.ResponseMessage}");
        });
    }
}
```

`Init` returns `-900` when the selected store does not match the running platform. All other public APIs return `-901` when called before initialization.

## Purchase Flow

Use the following sequence for each app session and purchase.

1. Call `HYPurchase.Init` once and wait for a successful callback.
2. At app launch, call `GetUnconsumedReceipts` to recover unfinished one-time purchases and `GetSubscribeReceipts` to restore current subscription entitlements.
3. Call `RefreshProducts` before presenting products for purchase. This is required on iOS because StoreKit caches products from this request.
4. Start a one-time purchase with `Purchase` or a subscription with `Subscribe`.
5. Send the successful `HyReceiptModel` to your backend and verify it with the relevant store service before granting content or entitlement.
6. After successful verification, complete the transaction:
   - Consumable: call `Consume`.
   - Android non-consumable: call `Acknowledge`.
   - iOS non-consumable or subscription: call `Acknowledge`; the iOS bridge maps this to transaction completion.
   - Android subscription: the native library acknowledges a successful subscription automatically.

Do not start another operation of the same type until its callback returns. In particular, serialize iOS `Purchase` and `Subscribe` calls, and serialize the two receipt-query calls.

## Response Format

Every callback returns a `HyResponseModel`.

```json
{
  "responseCode": 0,
  "responseMessage": "success",
  "responseObject": {}
}
```

`responseObject` is present only when an operation has a payload. A successful result uses response code `0` unless documented otherwise by the underlying store.

### Models

`HyProductModel`

| Field | Description |
| --- | --- |
| `Title` | Store product display title. |
| `ProductId` | Store product identifier. |
| `Price` | Numeric price supplied by the native store API. |
| `PriceString` | Localized display price. |
| `CurrencyCode` | ISO currency code. |
| `CurrencySymbol` | Localized currency symbol. |
| `Description` | Store product description. |
| `ProductType` | Product classification supplied by the native implementation. |

`HyReceiptModel`

| Field | Description |
| --- | --- |
| `ProductId` | Purchased product identifier. |
| `OrderId` | Store order or original transaction identifier. |
| `PurchaseToken` | Google Play purchase token. Empty on iOS. |
| `Payload` | UUID payload supplied during purchase, when available. |
| `ProductType` | Native product classification. |
| `PackageName`, `PurchaseTime` | Android-specific fields. |

Platform-specific fields can be empty or `null` on the other platform.

## Product Discovery

Call `RefreshProducts` before purchase. This is required on iOS because the native StoreKit implementation caches products returned by this call.

```csharp
var productIds = new List<string>
{
    "coins_100",
    "premium_upgrade",
    "monthly_subscription"
};

HYPurchase.RefreshProducts(productIds, response =>
{
    if (response.ResponseCode != 0 || response.ResponseObject == null)
    {
        Debug.LogError(response.ResponseMessage);
        return;
    }

    foreach (var product in response.ResponseObject)
    {
        Debug.Log($"{product.ProductId}: {product.Title} ({product.PriceString})");
    }
});
```

Passing `null` is treated as an empty product list on both platforms.

## Purchases

### One-time Purchases

```csharp
HYPurchase.Purchase("coins_100", response =>
{
    if (response.ResponseCode == 0)
    {
        Debug.Log($"Purchased: {response.ResponseObject.ProductId}");
    }
});
```

Use the payload overload when the purchase must be correlated with an account or server request. The payload must be a UUID string.

```csharp
var payload = System.Guid.NewGuid().ToString();

HYPurchase.Purchase("coins_100", payload, response =>
{
    Debug.Log($"Purchase: {response.ResponseCode} - {response.ResponseMessage}");
});
```

### Subscriptions

```csharp
var payload = System.Guid.NewGuid().ToString();

HYPurchase.Subscribe("monthly_subscription", payload, response =>
{
    Debug.Log($"Subscription: {response.ResponseCode} - {response.ResponseMessage}");
});
```

On Android, the native implementation selects the first eligible Google Play subscription offer. Configure a single eligible offer per product when the Unity API is used without an offer-token selection layer.

## Receipt Queries

```csharp
HYPurchase.GetUnconsumedReceipts(response =>
{
    if (response.ResponseCode == 0)
    {
        foreach (var receipt in response.ResponseObject ?? new List<HyReceiptModel>())
        {
            Debug.Log(receipt.ProductId);
        }
    }
});

HYPurchase.GetSubscribeReceipts(response =>
{
    if (response.ResponseCode == 0)
    {
        foreach (var receipt in response.ResponseObject ?? new List<HyReceiptModel>())
        {
            Debug.Log(receipt.ProductId);
        }
    }
});
```

`GetUnconsumedReceipts` returns unfinished one-time transactions. `GetSubscribeReceipts` returns current subscription entitlements.

## Completing Transactions

Verify every receipt with your backend before completing its transaction.

| Product type | Android | iOS |
| --- | --- | --- |
| Consumable | Call `Consume`. | Call `Consume`. |
| Non-consumable one-time product | Call `Acknowledge`. | Call `Acknowledge`. |
| Subscription | The Android native layer acknowledges a successful subscription. | Call `Acknowledge` after verification. |

```csharp
// After successful server-side verification of a consumable purchase.
HYPurchase.Consume("coins_100", response =>
{
    Debug.Log($"Consume: {response.ResponseCode} - {response.ResponseMessage}");
});

// After successful server-side verification of a non-consumable purchase.
HYPurchase.Acknowledge("premium_upgrade", response =>
{
    Debug.Log($"Acknowledge: {response.ResponseCode} - {response.ResponseMessage}");
});
```

`ConsumeAll` completes all currently owned Android in-app consumables or all unfinished iOS transactions. It returns one aggregated callback on Android.

## Store Country

```csharp
HYPurchase.GetStoreCountry(response =>
{
    if (response.ResponseCode == 0)
    {
        Debug.Log(response.ResponseObject.CountryCode);
    }
});
```

## Error Codes

Always inspect both `ResponseCode` and `ResponseMessage`. Native store response codes are intentionally preserved where possible.

### Unity Wrapper Errors

| Code | Meaning |
| --- | --- |
| `-900` | Store/platform mismatch, or iOS native common failure depending on call context. |
| `-901` | HYPurchase was not initialized. Also used by the iOS native layer for JSON parsing failures. |
| `-902` | Invalid UUID payload or Unity JSON parsing failure. The iOS native layer also uses it for refresh failures. |

### Android Errors

Android forwards Google Play Billing response codes and debug messages from the native layer. Common values are:

| Code | Google Play Billing meaning |
| --- | --- |
| `0` | OK |
| `1` | User canceled |
| `2` | Service unavailable |
| `3` | Billing unavailable |
| `4` | Item unavailable |
| `5` | Developer error |
| `6` | General error |
| `7` | Item already owned |
| `8` | Item not owned |
| `12` | Network error |
| `-1` | Service disconnected |
| `-2` | Feature not supported |
| `-3` | Service timeout (legacy) |
| `99` | Pending payment, emitted by HYPurchase native code |

The native layer can also return messages such as `SERVICE_UNAVAILABLE`, `ITEM_NOT_OWNED`, `item not found`, `subscription offer not found`, `already acknowledged`, and `initialization failed`.

### iOS Errors

The iOS native library returns these StoreKit-specific codes:

| Code | Meaning |
| --- | --- |
| `2` | Purchase canceled by the user. |
| `-801` | Purchase pending. |
| `-900` | Common failure. |
| `-901` | JSON parsing failed in the native layer. |
| `-902` | Product refresh failed. |
| `-903` | Receipt verification failed. |
| `-904` | Payload UUID parsing failed. |
| `-905` | Storefront was not available. |
| `-906` | App transaction verification failed. |
| `-907` | Operating-system version mismatch. |
| `-908` | App transaction instance could not be obtained. |
| `-909` | Product was removed or unavailable. |
| `-999` | Item was not found. |

## Concurrency

Do not start the same purchase operation more than once before its callback returns. The current Unity bridge stores one callback per operation type. On iOS, `Purchase` and `Subscribe` share a native callback slot, as do the two receipt-query APIs. Serialize those calls in application code.

## Integration Checklist

1. Build and copy the current Android AAR and iOS framework into `Assets/HYPurchase/Plugins`.
2. Initialize HYPurchase once with the platform-appropriate store.
3. Refresh products before initiating a purchase.
4. Verify purchase receipts on a trusted backend.
5. Call `Consume` or `Acknowledge` only after verification succeeds.
6. Serialize purchase and receipt-query calls until their callbacks return.
