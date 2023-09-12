#pragma warning disable CA1422

using Android.App;
using Android.Net.Wifi;
using System;
using System.Linq;
using System.Net;

namespace TasCon.Logic
{
    public static class WiFiInfo
    {
        public class WifiInfoResponse
        {
            public string Ssid { get; internal set; }
            public IPAddress IpAddress { get; internal set; }
        }

        public static WifiInfoResponse GetCurrentWiFi()
        {
            WifiManager ww = (WifiManager)Application.Context.GetSystemService("wifi");

            return new()
            {
                Ssid = ww.ConnectionInfo.SSID.Trim('"'),
                IpAddress = new IPAddress(BitConverter.GetBytes(ww.ConnectionInfo.IpAddress).ToArray())
            };
        }
    }
}
