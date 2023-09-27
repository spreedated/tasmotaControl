using Newtonsoft.Json;
using System;
using TasmotaQuery.Json;

namespace TasmotaQuery.Models
{
    public sealed record Wifi : StateBase
    {
        [JsonProperty("AP")]
        public short Ap { get; internal set; }

        [JsonProperty("SSId")]
        public string Ssid { get; internal set; }

        [JsonProperty("BSSId")]
        public string Bssid { get; internal set; }

        [JsonProperty("Channel")]
        public short Channel { get; internal set; }

        [JsonProperty("Mode")]
        public string Mode { get; internal set; }

        [JsonProperty("RSSI")]
        public short Rssi { get; internal set; }

        [JsonProperty("Signal")]
        public int Signal { get; internal set; }

        [JsonProperty("LinkCount")]
        public int LinkCount { get; internal set; }

        [JsonProperty("Downtime")]
        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan Downtime { get; internal set; }
    }
}
