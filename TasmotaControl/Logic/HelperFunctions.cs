using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TasCon.Models;

namespace TasCon.Logic
{
    public static class HelperFunctions
    {
        public static CorrectWiFiResponse IsCorrectWifi()
        {
            WiFiInfo.WifiInfoResponse w = WiFiInfo.GetCurrentWiFi();

            if (!typeof(Constants).GetFields(BindingFlags.Public | BindingFlags.Static).Where(x => x.Name.ToLower().StartsWith("allowed_wifi")).Select(y => y.GetValue(null).ToString()).Contains(w.Ssid))
            {
                return new()
                {
                    Ssid = w.Ssid
                };
            }

            return new()
            {
                Ssid = w.Ssid,
                IsAllowed = true
            };
        }

        public static void RequestPermissions()
        {
            ActivityCompat.RequestPermissions(Platform.CurrentActivity, new[]
            {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.Internet,
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.ChangeWifiState,
            Manifest.Permission.AccessNetworkState
        }, 0);
        }

        public static bool ArePermissionsGiven()
        {
            Dictionary<string, bool> permissionCheck = new()
            {
                { Manifest.Permission.AccessCoarseLocation, false },
                { Manifest.Permission.AccessFineLocation, false },
                { Manifest.Permission.Internet, false },
                { Manifest.Permission.AccessWifiState, false },
                { Manifest.Permission.ChangeWifiState, false },
                { Manifest.Permission.AccessNetworkState, false }
            };

            foreach (string perm in permissionCheck.Select(x => x.Key))
            {
                permissionCheck[perm] = ActivityCompat.CheckSelfPermission(Platform.AppContext, perm) == Permission.Granted;
            }

            return permissionCheck.Any(x => !x.Value);
        }
    }
}
