using Android.Content;

namespace TasCon.Logic
{
    public class WifiServiceControl : IWifiServiceControl
    {
        public void StartService()
        {
            var serviceIntent = new Intent(Android.App.Application.Context, typeof(WifiMonitoringService));
            Android.App.Application.Context.StartForegroundService(serviceIntent);
        }

        public void StopService()
        {
            var serviceIntent = new Intent(Android.App.Application.Context, typeof(WifiMonitoringService));
            Android.App.Application.Context.StopService(serviceIntent);
        }
    }
}
