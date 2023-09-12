using Microsoft.Maui.Controls;
using neXn.Lib.Maui.ViewLogic;
using neXn.Lib.Strings.Extensions;
using System.Reflection;
using System.Windows.Input;
using TasCon.Logic;
using TasCon.Views;

namespace TasCon.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        public string CustomTitle { get; } = $"{typeof(AppShell).Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title} v{typeof(AppShell).Assembly.GetName().Version.ToNiceString()}";
        public ICommand DeviceManagerCommand { get; } = new Command(async () =>
        {
            Shell.Current.FlyoutIsPresented = false;
            await App.Current.MainPage.Navigation.PushAsync(new DeviceManager());
        });

        public ICommand AboutCommand { get; } = new Command(async (s) =>
        {
            Shell.Current.FlyoutIsPresented = false;
            await App.Current.MainPage.Navigation.PushAsync(RuntimeStorage.PreloadedPages.Find(x => x.GetType() == typeof(About)));
        });
    }
}
