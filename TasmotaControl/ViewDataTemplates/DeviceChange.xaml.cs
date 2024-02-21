using Microsoft.Maui.Controls;
using System.Linq;
using System.Net;
using System.Windows.Input;
using TasCon.Logic;
using TasCon.Resources.Strings;
using TasCon.Views;

namespace TasCon.ViewDataTemplates;

public partial class DeviceChange : ContentView
{
    public ICommand RemoveCommand { get; } = new Command<IPAddress>(async (d) =>
    {
        bool ans = await Shell.Current.CurrentPage.DisplayAlert(AppStrings.REMOVE_DEVICE, $"{AppStrings.ARE_YOU_SURE_TO_REMOVE}\n\"{d}\"?", AppStrings.YES, AppStrings.NO);
        
        if (ans && RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Exists(x => x.Address == d))
        {
            RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Remove(RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.First(x => x.Address == d));

            await ((DeviceManager)Shell.Current.CurrentPage).RefreshDeviceList();
            MainPage.StaticInstance.RefreshDeviceList();
            RuntimeStorage.ConfigurationHandler.Save();
        }
    });

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
        }
    }

    public DeviceChange()
    {
        this.InitializeComponent();
        this.BindingContext = this;
        this.Instance = this;
    }
}