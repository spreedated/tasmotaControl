#pragma warning disable CA1422

using Android.App;
using Android.Net.Wifi;
using System;
using System.Linq;
using System.Net;
using TasCon.Models;

namespace TasCon.Logic
{
    public static class WiFiInfo
    {
        public static WiFiInformation GetCurrentWiFi()
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
