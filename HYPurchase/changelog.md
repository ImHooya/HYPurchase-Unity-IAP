# Changelog

All notable changes to HYPurchase Unity IAP are documented in this file.

## 1.1.0

### Changed

- Updated the Android integration to Google Play Billing Client `9.1.0`.
- Updated the Android Gradle template to declare `com.android.billingclient:billing:9.1.0` and Kotlin serialization support.
- Updated product and receipt model mapping to keep Android and iOS response fields aligned.

### Added

- Subscription purchase support through `HYPurchase.Subscribe`.
- Subscription entitlement lookup through `HYPurchase.GetSubscribeReceipts`.
- Store country lookup through `HYPurchase.GetStoreCountry`.
- Native callback handling for subscription purchases, subscription receipts, and store country responses on Android and iOS.

### Notes

- Android builds require Java 17 or newer when using the included `mainTemplate.gradle`.
- On Android, `Subscribe` uses the first eligible offer returned by Google Play. Configure subscription offers accordingly.

## 1.0.0 - 2026-01-12

- Initial public release.
