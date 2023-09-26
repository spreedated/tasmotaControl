using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public sealed class Sensors
    {
        [JsonProperty("Time")]
        public DateTime Time { get; set; }

        [JsonProperty("TempUnit")]
        public char TempUnit { get; set; }

        [JsonIgnore()]
        public float Temperature1 { get; set; }

        [JsonIgnore()]
        public DateTime QueryTime { get; set; }
    }
}
