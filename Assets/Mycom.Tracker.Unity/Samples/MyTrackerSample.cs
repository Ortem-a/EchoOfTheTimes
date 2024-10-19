using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace Mycom.Tracker.Unity.Samples
{
    public class MyTrackerSample : MonoBehaviour
    {
        private const String DebugToggleName = "Debug";
        private const String EventButtonName = "Event";
        private const String InitButtonName = "Init";
        private const String InviteButtonName = "Invite";
        private const String LevelButtonName = "Level";
        private const String LoginButtonName = "Login";
        private const String Param1ToggleName = "Param1";
        private const String Param2ToggleName = "Param2";
        private const String PurchaseButtonName = "Purchase";
        private const String RegistrationButtonName = "Registration";
        private const String TrackerEventName = "CustomEvent";

#if UNITY_IOS
        private const String TrackerId = "97953241358447035268";
#elif UNITY_ANDROID
        private const String TrackerId = "89232805149757155048";
#else
        private const String TrackerId = "";
#endif

        private const Int32 TrackerLevel = 42;

        private readonly IDictionary<String, String> _paramsDictionary = new Dictionary<String, String>();

        public void Awake()
        {
            Debug.Log("Awake");

            MyTracker.IsDebugMode = true;

            var myTrackerParams = MyTracker.MyTrackerParams;
            myTrackerParams.Age = 21;
            myTrackerParams.Gender = GenderEnum.Male;
            myTrackerParams.VkConnectId = "vk_connect_id";

            var myTrackerConfig = MyTracker.MyTrackerConfig;
            myTrackerConfig.IsTrackingLaunchEnabled = true;

            myTrackerConfig.RegisterForSkAdAttribution = false;

            MyTracker.SetAttributionListener(attribution => Debug.Log("Attribution: " + attribution.Deeplink));

            foreach (var button in FindObjectsOfType<Button>())
            {
                var buttonClickedEvent = new Button.ButtonClickedEvent();

                switch (button.name)
                {
                    case EventButtonName:
                        buttonClickedEvent.AddListener(() => MyTracker.TrackEvent(TrackerEventName, _paramsDictionary));
                        break;
                    case InviteButtonName:
                        buttonClickedEvent.AddListener(() => MyTracker.TrackInviteEvent(_paramsDictionary));
                        break;
                    case LevelButtonName:
                        buttonClickedEvent.AddListener(() => MyTracker.TrackLevelEvent(TrackerLevel, _paramsDictionary));
                        break;
                    case LoginButtonName:
                        buttonClickedEvent.AddListener(() => MyTracker.TrackLoginEvent("unity_login_user_id", null, _paramsDictionary));
                        break;
                    case RegistrationButtonName:
                        buttonClickedEvent.AddListener(() => MyTracker.TrackRegistrationEvent("unity_registration_user_id", null, _paramsDictionary));
                        break;
#if UNITY_PURCHASING && UNITY_ANDROID
                    case PurchaseButtonName:
                        buttonClickedEvent.AddListener(() =>
                        {
                            var productConstuctorInfo = typeof(Product).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                                                                                       null,
                                                                                       new[]
                                                                                       {
                                                                                           typeof (ProductDefinition),
                                                                                           typeof (ProductMetadata)
                                                                                       },
                                                                                       null);

                            if (productConstuctorInfo == null)
                            {
                                return;
                            }

                            var productDefinition = new ProductDefinition("UnityId", "StoreId", ProductType.Consumable);
                            var productMetadata = new ProductMetadata("100USD", "ProductTitle", "ProductDescription", "USD", 100);

                            var product = (Product)productConstuctorInfo.Invoke(new System.Object[] { productDefinition, productMetadata });

                            var receiptProperty = typeof(Product).GetProperty("receipt");
                            if (receiptProperty == null)
                            {
                                return;
                            }

                            var receiptSetMethod = receiptProperty.GetSetMethod(true);
                            if (receiptSetMethod == null)
                            {
                                return;
                            }

                            var receiptString = "{\"Store\":\"GooglePlay\",\"TransactionID\":\"GPA.3347-8986-0030-28878\",\"Payload\":\"{\\\"json\\\":\\\"{\\\\\\\"orderId\\\\\\\":\\\\\\\"GPA.3347-8986-0030-28878\\\\\\\",\\\\\\\"packageName\\\\\\\":\\\\\\\"com.hunterhamster.snailbob3\\\\\\\",\\\\\\\"productId\\\\\\\":\\\\\\\"money_slot1\\\\\\\",\\\\\\\"purchaseTime\\\\\\\":1572616177167,\\\\\\\"purchaseState\\\\\\\":0,\\\\\\\"developerPayload\\\\\\\":\\\\\\\"{\\\\\\\\\\\\\\\"developerPayload\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\"is_free_trial\\\\\\\\\\\\\\\":false,\\\\\\\\\\\\\\\"has_introductory_price_trial\\\\\\\\\\\\\\\":false,\\\\\\\\\\\\\\\"is_updated\\\\\\\\\\\\\\\":false,\\\\\\\\\\\\\\\"accountId\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"\\\\\\\\\\\\\\\"}\\\\\\\",\\\\\\\"purchaseToken\\\\\\\":\\\\\\\"dpdmijkpddblhkalgoimlmeh.AO-J1OyEojbt7VhWJpPIsi8kQSnzgTS5-PTANdu84rHPVsL_kyJRQ1QW8pn2L4zsNZLMLY59uJhfsATJ_mTxvtSh3JjsuavS6JXmKLMhR0OL_vQS1-O7PTQczWa0fqjLc4DxQ6vC_haJ\\\\\\\"}\\\",\\\"signature\\\":\\\"fAo+6ZY3RnvLjIjb8x6qM6RTo\\\\/9aKpjfdOLie6\\\\/q6AWY3FfMAEgt0z+Tdr6jiZGGKbGAOtD5RbEMf3xfCIhQ4qsTNLkmWiYF58rp1+RSi+BjSNVw3ZoH3vNXMYHcmiFnel0g4mbhlEtVcGSRj0QPDsac69Azb24MzZPl1Id7AxtTIMJS9Z7ePY3MKjQ2h9vgqmV9tlJcubrMjrdmTyPhax96FR7mbmebHXfTVNIl+3KHq3B7diSbTi8t9jKiMrw2juJe456L55V4E0k6TiBdkRp3wvvBlQufU281mn51ixyYF9fdhEUM4WWJumXS5HMTpJdkx+n3D4N+RSVKBcplFg==\\\",\\\"skuDetails\\\":\\\"{\\\\\\\"skuDetailsToken\\\\\\\":\\\\\\\"AEuhp4JQbO0snzoGkwzOExCxhqw14SLpArZVhYh45M33SacsFz-xftrlVN241c_FTjI=\\\\\\\",\\\\\\\"productId\\\\\\\":\\\\\\\"money_slot1\\\\\\\",\\\\\\\"type\\\\\\\":\\\\\\\"inapp\\\\\\\",\\\\\\\"price\\\\\\\":\\\\\\\"129,00 \u20BD\\\\\\\",\\\\\\\"price_amount_micros\\\\\\\":129000000,\\\\\\\"price_currency_code\\\\\\\":\\\\\\\"RUB\\\\\\\",\\\\\\\"title\\\\\\\":\\\\\\\"Money Slot 1 (Snail Bob 3)\\\\\\\",\\\\\\\"description\\\\\\\":\\\\\\\"Money Slot 1\\\\\\\"}\\\",\\\"isPurchaseHistorySupported\\\":true}\"}";

                            receiptSetMethod.Invoke(product, new[] { receiptString });

                            MyTracker.TrackPurchaseEvent(product, _paramsDictionary);
                        });
                        break;
#endif
                    case InitButtonName:
                        buttonClickedEvent.AddListener(() => MyTracker.Init(TrackerId));
                        break;
                    default:
                        continue;
                }

                button.onClick = buttonClickedEvent;
            }

            foreach (var toggle in FindObjectsOfType<Toggle>())
            {
                var toggleEvent = new Toggle.ToggleEvent();
                switch (toggle.name)
                {
                    case DebugToggleName:
                        toggleEvent.AddListener(value => MyTracker.IsDebugMode = value);
                        toggle.isOn = MyTracker.IsDebugMode;
                        break;
                    case Param1ToggleName:
                        toggleEvent.AddListener(value =>
                                                {
                                                    const String key = "Name1";
                                                    if (value)
                                                    {
                                                        _paramsDictionary[key] = "Value1";
                                                    }
                                                    else
                                                    {
                                                        _paramsDictionary.Remove(key);
                                                    }
                                                });
                        toggle.isOn = false;
                        break;
                    case Param2ToggleName:
                        toggleEvent.AddListener(value =>
                                                {
                                                    const String key = "Name2";
                                                    if (value)
                                                    {
                                                        _paramsDictionary[key] = "Value2";
                                                    }
                                                    else
                                                    {
                                                        _paramsDictionary.Remove(key);
                                                    }
                                                });
                        toggle.isOn = false;
                        break;
                    default:
                        continue;
                }
                toggle.onValueChanged = toggleEvent;
            }
        }
    }
}