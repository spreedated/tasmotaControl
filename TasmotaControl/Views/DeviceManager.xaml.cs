using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using TasCon.Logic;

namespace TasCon.Views;

public partial class DeviceManager : ContentPage
{
    public ICommand AddCommand { get; } = new Command<Page>(async (p) =>
    {
        string input = await p.DisplayPromptAsync("Add device", $"Please enter an IPv4 address.", initialValue: "192.168.10.");

        if (string.IsNullOrEmpty(input) || !IPAddress.TryParse(input, out IPAddress ipadd) || RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Exists(x => x.Address == ipadd))
        {
            return;
        }

        await Task.Factory.StartNew(async () =>
        {
            RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Add(new(input));

            await ((DeviceManager)p).RefreshDeviceList();

            MainPage.StaticInstance.RefreshDeviceList();
            RuntimeStorage.ConfigurationHandler.Save();
        });
    });

    public ICommand SetViewPriority { get; } = new Command<DeviceManager>((dm) =>
    {
        short viewPrio = 0;
        foreach (TasmotaDevice td in dm.ThisDevices)
        {
            td.ViewPriority = viewPrio;
            viewPrio++;
        }

        RuntimeStorage.ConfigurationHandler.Save();
    });

    private bool _IsLoading = true;
    public bool IsLoading
    {
        get
        {
            return this._IsLoading;
        }
        set
        {
            this._IsLoading = value;
            base.OnPropertyChanged(nameof(this.IsLoading));
        }
    }

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
        this.Dispatcher.Dispatch(async () =>
        {
            await this.RefreshDeviceList();
        });
    }

    private ObservableCollection<TasmotaDevice> _ThisDevices;
    public ObservableCollection<TasmotaDevice> ThisDevices
    {
        get
        {
            return this._ThisDevices;
        }
        set
        {
            this._ThisDevices = value;
            base.OnPropertyChanged(nameof(this.ThisDevices));
        }
    }

    public async Task RefreshDeviceList()
    {
        this.IsLoading = true;

        await Task.Factory.StartNew(async () =>
        {
            this.ThisDevices = new(RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.OrderBy(x => x.ViewPriority).ThenBy(y => y.Address));
            await Task.Delay(1500);
            this.IsLoading = false;
        });
    }
}