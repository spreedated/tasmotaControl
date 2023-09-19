using Newtonsoft.Json;
using System;
using TasmotaQuery.Json;

namespace TasmotaQuery.Models
{
    public sealed class Wifi
    {
        [JsonProperty("AP")]
        public short Ap { get; set; }

        [JsonProperty("SSId")]
        public string Ssid { get; set; }

        [JsonProperty("BSSId")]
        public string Bssid { get; set; }

        [JsonProperty("Channel")]
        public short Channel { get; set; }

        [JsonProperty("Mode")]
        public string Mode { get; set; }

        [JsonProperty("RSSI")]
        public short Rssi { get; set; }

        [JsonProperty("Signal")]
        public int Signal { get; set; }

        [JsonProperty("LinkCount")]
        public int LinkCount { get; set; }

        [JsonProperty("Downtime")]
        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan Downtime { get; set; }

        [JsonIgnore]
        public DateTime QueryTime { get; set; }
    }
}
