using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TasCon.Logic;
using TasCon.Platforms.Android;
using TasCon.ViewElements;
using TasCon.ViewModels;

namespace TasCon.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();

        ((MainPageViewModel)this.BindingContext).Instance = this;

        RequestPermissions();

        ((MainPageViewModel)this.BindingContext).NoPermission = ArePermissionsGiven();

        if (!((MainPageViewModel)this.BindingContext).NoPermission)
        {
            WiFiInfo.WifiInfoResponse w = WiFiInfo.GetCurrentWiFi();
            ((MainPageViewModel)this.BindingContext).WiFiSSID = w.Ssid;

            if (!typeof(Constants).GetFields(BindingFlags.Public | BindingFlags.Static).Where(x => x.Name.ToLower().StartsWith("allowed_wifi")).Select(y => y.GetValue(null).ToString()).Contains(w.Ssid))
            {
                return;
            }

            ((MainPageViewModel)this.BindingContext).IsCorrectWifi = true;

            foreach (TasmotaDevice device in RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices)
            {
                DeviceContent dc = new()
                {
                    Device = device,
                    Margin = new Microsoft.Maui.Thickness(0, 0, 0, 12)
                };

                this.DeviceLayout.Children.Add(dc);
            }
        }
    }

    public async Task RefreshDevices()
    {
        List<Task> deviceRefreshing = new();

        foreach (DeviceContent dc in this.DeviceLayout.Children.Where(y => y.GetType() == typeof(DeviceContent)).Select(x => (DeviceContent)x))
        {
            deviceRefreshing.Add(Task.Factory.StartNew(async () => await dc.DeviceRefresh()));
        }

        await Task.WhenAll(deviceRefreshing);
    }

    private static void RequestPermissions()
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

    private static bool ArePermissionsGiven()
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

    private void StartServiceButton_Clicked(object sender, EventArgs e)
    {
        // Start the background service
        //DependencyService.Get<IWifiServiceControl>().StartService();

        //WifiMonitoringService.WifiReceiver w = new(new());
        //w.OnReceive(null, null);

        AndroidServiceManager.StartMyService();
    }

    private void StopServiceButton_Clicked(object sender, EventArgs e)
    {
        // Stop the background service
        DependencyService.Get<IWifiServiceControl>().StopService();
    }
}

