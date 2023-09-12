using Android.App;
using Android.Content;

namespace TasCon.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    internal class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //if (intent.Action == Intent.ActionBootCompleted)
            //{
            //    Toast.MakeText(context, "Boot completed event received", ToastLength.Short).Show();

            //    var serviceIntent = new Intent(context, typeof(WifiMonitoringService));

            //    ContextCompat.StartForegroundService(context, serviceIntent);
            //}
        }
    }
}
