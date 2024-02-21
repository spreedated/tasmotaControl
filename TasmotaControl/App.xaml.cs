#define DELETEALLDEVICES

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;
using TasCon.Logic;

namespace TasCon;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
#if DELETEALLDEVICES && DEBUG
        if (File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, "config.json")))
        {
            File.Delete(Path.Combine(FileSystem.Current.AppDataDirectory, "config.json"));
        }
#endif

        RuntimeStorage.ConfigurationHandler = new(new(Path.Combine(FileSystem.Current.AppDataDirectory, "config.json")));
        RuntimeStorage.ConfigurationHandler.Load();

#if DELETEALLDEVICES && DEBUG
        RuntimeStorage.ConfigurationHandler.RuntimeConfiguration.Devices.Clear();
#endif

        this.MainPage = new AppShell();

        Task.Factory.StartNew(() =>
        {
            RuntimeStorage.PreloadedPages.Add(new Views.About());
        });
    }
}
