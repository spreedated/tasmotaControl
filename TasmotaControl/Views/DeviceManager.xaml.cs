using Microsoft.Maui.Controls;
using System.Linq;
using System.Net;
using System.Windows.Input;
using TasCon.Logic;
using TasCon.ViewElements;

namespace TasCon.Views;

public partial class DeviceManager : ContentPage
{
    public ICommand AddCommand { get; } = new Command<Page>(async (p) =>
    {
        string input = await p.DisplayPromptAsync("Add device", $"Please enter an IPv4 address.", initialValue: "192.168.10.");

        if (string.IsNullOrEmpty(input) || !IPAddress.TryParse(input, out _) || RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Exists(x => x.Address == input))
        {
            return;
        }

        RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Add(new(input));

        ((DeviceManager)p).RefreshDeviceList();
        MainPage.StaticInstance.RefreshDeviceList();
        RuntimeStorage.ConfigurationHandler.Save();
    });

    private ContentPage _Instance;
    public ContentPage Instance
    {
        get
        {
            return this._Instance;
        }
        set
        {
            this._Instance = value;
            base.OnPropertyChanged(nameof(this.Instance));
        }
    }

    public DeviceManager()
    {
        this.InitializeComponent();
        this.Instance = this;
        this.RefreshDeviceList();
    }

    public void RefreshDeviceList()
    {
        this.DeviceList.Clear();

        foreach (TasmotaDevice dev in RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.OrderBy(x => x.ViewPriority).ThenBy(y => y.Address))
        {
            DeviceChange dc = new()
            {
                Device = dev,
                ParentPage = this,
                Margin = new Microsoft.Maui.Thickness(0, 0, 0, 12)
            };

            this.DeviceList.Add(dc);
        }
    }
}