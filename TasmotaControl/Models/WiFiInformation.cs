using System.Net;

namespace TasCon.Models
{
    public sealed class WiFiInformation
    {
        public string Ssid { get; set; }
        public IPAddress IpAddress { get; set; }
    }
}
