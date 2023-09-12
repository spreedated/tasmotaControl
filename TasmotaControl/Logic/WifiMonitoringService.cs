#pragma warning disable

using Android;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using AndroidX.Core.App;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TasCon.Platforms.Android;
using static TasCon.Logic.WiFiInfo;

namespace TasCon.Logic
{
    [Service]
    public class WifiMonitoringService : Service
    {
        private int myId = new object().GetHashCode();
        private int BadgeNumber = 0;
        private ConnectivityManager connectivityManager;
        private WifiReceiver wifiReceiver;

        public static readonly string ChannelId = "someChannelhere";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.Immutable);

            var notification = new NotificationCompat.Builder(this, ChannelId).SetContentText("Hello").SetSmallIcon(Resource.Drawable.navigation_empty_icon).SetContentIntent(pendingIntent);

            var input = intent.GetStringExtra("inputExtra");

            connectivityManager = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            wifiReceiver = new WifiReceiver(this);

            RegisterReceiver(wifiReceiver, new IntentFilter(ConnectivityManager.ConnectivityAction));

            AndroidServiceManager.IsRunning = true;
            BadgeNumber++;

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            this.UnregisterReceiver(wifiReceiver);
        }

        public void UpdateWifiInfo(string ssid)
        {
            throw new NotImplementedException();
        }

        // Reset Wi-Fi SSID label in the UI
        public void ResetWifiInfo()
        {
            throw new NotImplementedException();
        }

        // Perform action when connected to "nxn-fritz"
        public void PerformActionOnWifiConnected()
        {
            throw new NotImplementedException();
        }

        public static string ConvertFromIntegerToIpAddress(uint ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);

            // flip little-endian to big-endian(network order)
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return new IPAddress(bytes).ToString();
        }

        public class WifiReceiver : BroadcastReceiver
        {
            private readonly WifiMonitoringService service;

            public WifiReceiver(WifiMonitoringService service)
            {
                this.service = service;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                //var conn = context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;

                var conn = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.ConnectivityService)!;

                var wifiInfo = conn.ActiveNetworkInfo;

                WifiManager ww = (WifiManager)Android.App.Application.Context.GetSystemService(WifiService);

                var oo = ww.ConnectionInfo.SSID;

                var pp = new IPAddress(BitConverter.GetBytes(ww.ConnectionInfo.IpAddress).ToArray()).ToString();

                if (wifiInfo != null && wifiInfo.Type == ConnectivityType.Wifi)
                {
                    string ssid = wifiInfo.ExtraInfo;

                    if (ssid != null && ssid.Equals("\"nxn-fritz\""))
                    {
                        // Perform your desired action when connected to "nxn-fritz"
                        service.PerformActionOnWifiConnected();
                    }

                    // Update the SSID label in the UI
                    (service as WifiMonitoringService)?.UpdateWifiInfo(ssid);
                }
                else
                {
                    // Not connected to a Wi-Fi network
                    (service as WifiMonitoringService)?.ResetWifiInfo();
                }
            }
        }
    }
}
