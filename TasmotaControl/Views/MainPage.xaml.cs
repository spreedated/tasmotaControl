using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasCon.Logic;
using TasCon.Platforms.Android;
using TasCon.ViewElements;
using TasCon.ViewModels;
using static TasCon.Logic.HelperFunctions;

namespace TasCon.Views;

public partial class MainPage : ContentPage
{
    public static MainPage StaticInstance { get; private set; }

    public MainPage()
    {
        this.InitializeComponent();
        ((MainPageViewModel)this.BindingContext).Instance = this;
        StaticInstance = this;

        RequestPermissions();

        ((MainPageViewModel)this.BindingContext).NoPermission = ArePermissionsGiven();

        if (!((MainPageViewModel)this.BindingContext).NoPermission)
        {
            this.RefreshDeviceList();
        }
    }

    public void RefreshDeviceList()
    {
        this.DeviceLayout.Children.Clear();

        this.RefreshWiFiInfo();

        if (((MainPageViewModel)this.BindingContext).IsCorrectWifi)
        {
            foreach (TasmotaDevice device in RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.OrderBy(x => x.ViewPriority).ThenBy(y => y.Address))
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

    private void RefreshWiFiInfo()
    {
        Models.CorrectWiFiResponse cw = IsCorrectWifi();

        ((MainPageViewModel)this.BindingContext).WiFiSSID = cw.Ssid;
        ((MainPageViewModel)this.BindingContext).IsCorrectWifi = cw.IsAllowed;
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

