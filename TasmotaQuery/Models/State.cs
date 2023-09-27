using Newtonsoft.Json;
using System;
using TasmotaQuery.Json;

namespace TasmotaQuery.Models
{
    public sealed record State : StateBase
    {
        [JsonProperty("Time")]
        public DateTime PowerOnState { get; internal set; }

        [JsonProperty("Uptime")]
        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan Uptime { get; internal set; }

        [JsonProperty("UptimeSec")]
        public long UptimeSec { get; internal set; }

        [JsonProperty("Heap")]
        public int Heap { get; internal set; }

        [JsonProperty("SleepMode")]
        public string SleepMode { get; internal set; }

        [JsonProperty("Sleep")]
        public int Sleep { get; internal set; }

        [JsonProperty("LoadAvg")]
        public int LoadAvg { get; internal set; }

        [JsonProperty("MqttCount")]
        public int MqttCount { get; internal set; }

        [JsonProperty("Wifi")]
        public Wifi Wifi { get; internal set; }
    }
}
