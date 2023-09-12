using Microsoft.Maui.Controls;
using TasCon.Logic;
using TasCon.ViewElements;

namespace TasCon.Views;

public partial class DeviceManager : ContentPage
{
    public DeviceManager()
    {
        this.InitializeComponent();

        foreach (TasmotaDevice dev in RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices)
        {
            DeviceChange dc = new()
            {
                Device = dev,
                Margin = new Microsoft.Maui.Thickness(0, 0, 0, 12)
            };

            this.DeviceList.Add(dc);
        }
    }

    private async void Button_Clicked(object sender, System.EventArgs e)
    {
        await base.Navigation.PushAsync(new MainPage());
    }
}