using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using TasCon.Logic;
using TasCon.Views;

namespace TasCon.ViewElements;

public partial class DeviceChange : ContentView
{
    public ICommand ShutterCheckedCommand { get; } = new Command<ContentView>((p) =>
    {
        ((DeviceChange)p).ShutterIsChecked ^= true;
    });

    public ICommand TempSensCheckedCommand { get; } = new Command<ContentView>((p) =>
    {
        ((DeviceChange)p).TempSensIsChecked ^= true;
    });

    public ICommand RemoveCommand { get; } = new Command<RemoveCommandObject>(async (d) =>
    {
        bool ans = await d.ParentPage.DisplayAlert("Remove device", $"Are you sure to remove\n\"{((DeviceChange)d.Instance).Device.Address}\"?", "Yes", "No");
        if (ans)
        {
            RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Remove(((DeviceChange)d.Instance).Device);

            d.ParentPage.RefreshDeviceList();
            MainPage.StaticInstance.RefreshDeviceList();
            RuntimeStorage.ConfigurationHandler.Save();
        }
    });

    public ICommand OrderUpCommand { get; } = new Command<ContentView>((v) =>
    {
        ((DeviceChange)v).Device.ViewPriority--;
        ((DeviceChange)v).ParentPage.RefreshDeviceList();
        MainPage.StaticInstance.RefreshDeviceList();
    });

    public ICommand OrderDownCommand { get; } = new Command<ContentView>((v) =>
    {
        ((DeviceChange)v).Device.ViewPriority++;
        ((DeviceChange)v).ParentPage.RefreshDeviceList();
        MainPage.StaticInstance.RefreshDeviceList();
    });

    private TasmotaDevice _Device;
    public TasmotaDevice Device
    {
        get
        {
            return this._Device;
        }
        set
        {
            this._Device = value;
            base.OnPropertyChanged(nameof(this.Device));
            this.AdjustDeviceOptionsToViewModel();
        }
    }

    private DeviceManager _ParentPage;
    public DeviceManager ParentPage
    {
        get
        {
            return this._ParentPage;
        }
        set
        {
            this._ParentPage = value;
            base.OnPropertyChanged(nameof(this.ParentPage));
            base.OnPropertyChanged(nameof(this.RemoveCommandObj));
        }
    }

    private ContentView _Instance;
    public ContentView Instance
    {
        get
        {
            return this._Instance;
        }
        set
        {
            this._Instance = value;
            base.OnPropertyChanged(nameof(this.Instance));
            base.OnPropertyChanged(nameof(this.RemoveCommandObj));
        }
    }

    private bool _ShutterIsChecked;
    public bool ShutterIsChecked
    {
        get
        {
            return this._ShutterIsChecked;
        }
        set
        {
            this._ShutterIsChecked = value;
            base.OnPropertyChanged(nameof(this.ShutterIsChecked));
            this.ApplyOptionsToDevice();
        }
    }

    private bool _TempSensIsChecked;
    public bool TempSensIsChecked
    {
        get
        {
            return this._TempSensIsChecked;
        }
        set
        {
            this._TempSensIsChecked = value;
            base.OnPropertyChanged(nameof(this.TempSensIsChecked));
            this.ApplyOptionsToDevice();
        }
    }

    public RemoveCommandObject RemoveCommandObj
    {
        get
        {
            return new()
            {
                Instance = this.Instance,
                ParentPage = this.ParentPage
            };
        }
    }

    public class RemoveCommandObject
    {
        public ContentView Instance { get; set; }
        public DeviceManager ParentPage { get; set; }
    }

    public DeviceChange()
    {
        this.InitializeComponent();
        this.BindingContext = this;
        this.Instance = this;
    }

    private void AdjustDeviceOptionsToViewModel()
    {
        this._ShutterIsChecked = this.Device.IsShutter;
        this._TempSensIsChecked = this.Device.HasTemperature;
        base.OnPropertyChanged(nameof(this.ShutterIsChecked));
        base.OnPropertyChanged(nameof(this.TempSensIsChecked));
    }

    private void ApplyOptionsToDevice()
    {
        this.Device.IsShutter = this.ShutterIsChecked;
        this.Device.HasTemperature = this.TempSensIsChecked;

        Task.Factory.StartNew(() =>
        {
            RuntimeStorage.ConfigurationHandler.Save();
        });
    }
}