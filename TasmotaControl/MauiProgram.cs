using CommunityToolkit.Maui;
using MauiIcons.Material;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using TasCon.Logic;

namespace TasCon;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        DependencyService.Register<IWifiServiceControl, WifiServiceControl>();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMaterialMauiIcons()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("ProximaNova-BlackIt.otf", "ProximaNovaBlackItalic");
                fonts.AddFont("ProximaNova-ExtrabldIt.otf", "ProximaNovaExtraboldItalic");
                fonts.AddFont("ProximaNova-Extrabold.otf", "ProximaNovaExtrabold");
                fonts.AddFont("ProximaNova-LightIt.otf", "ProximaNovaLightItalic");
                fonts.AddFont("ProximaNova-Reg.otf", "ProximaNovaRegular");
                fonts.AddFont("ProximaNova-RegIt.otf", "ProximaNovaRegularItalic");
                fonts.AddFont("nothing-font-5x7.ttf", "Nothing");
            });

        return builder.Build();
    }
}
