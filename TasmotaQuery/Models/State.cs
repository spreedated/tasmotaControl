using Newtonsoft.Json;
using System;
using TasmotaQuery.Json;

namespace TasmotaQuery.Models
{
    public sealed class State
    {
        [JsonProperty("Time")]
        public DateTime PowerOnState { get; set; }

        [JsonProperty("Uptime")]
        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan Uptime { get; set; }

        [JsonProperty("UptimeSec")]
        public long UptimeSec { get; set; }

        [JsonProperty("Heap")]
        public int Heap { get; set; }

        [JsonProperty("SleepMode")]
        public string SleepMode { get; set; }

        [JsonProperty("Sleep")]
        public int Sleep { get; set; }

        [JsonProperty("LoadAvg")]
        public int LoadAvg { get; set; }

        [JsonProperty("MqttCount")]
        public int MqttCount { get; set; }

        [JsonProperty("Wifi")]
        public Wifi Wifi { get; set; }

        [JsonIgnore]
        public DateTime QueryTime { get; set; }
    }
}
