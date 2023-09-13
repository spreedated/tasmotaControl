using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace TasCon;

[Application(UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }

    public override void OnCreate()
    {
        base.OnCreate();

        //var serviceChannel = new NotificationChannel("someChannelhere", "My Background Service Channel", NotificationImportance.High);

        //if (GetSystemService(NotificationService) is NotificationManager manager)
        //{
        //    manager.CreateNotificationChannel(serviceChannel);
        //}
    }
}
