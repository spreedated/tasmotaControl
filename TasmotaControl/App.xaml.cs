//#define DELETEALLDEVICES

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TasCon.Logic;
using static TasCon.Logic.Constants;

namespace TasCon;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        RuntimeStorage.ConfigurationHandler = new(new(Path.Combine(FileSystem.Current.AppDataDirectory, "config.json")));
        RuntimeStorage.ConfigurationHandler.Load();

#if DELETEALLDEVICES
        RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Clear();
#endif

#if DEBUG
        if (!RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Any())
        {
            RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.AddRange(new TasmotaDevice[]
            {
                new("192.168.1.211"),
                new(ADDRESS_WOHNWAND),
                //new(ADDRESS_SOFA),
                //new(ADDRESS_FRONTDOOR),
                new(ADDRESS_NSPANEL) { IsShutter = true, HasTemperature = true },
                //new(ADDRESS_SCHLAFZIMMER),
                //new(ADDRESS_SCHLAFZIMMERSHUTTER) { IsShutter = true }
            });
            RuntimeStorage.ConfigurationHandler.Save();
        }
#endif

        this.MainPage = new AppShell();

        Task.Factory.StartNew(() =>
        {
            RuntimeStorage.PreloadedPages.Add(new Views.About());
        });
    }
}
