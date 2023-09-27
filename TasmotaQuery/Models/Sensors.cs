using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public sealed record Sensors : StateBase
    {
        [JsonProperty("Time")]
        public DateTime Time { get; internal set; }

        [JsonProperty("TempUnit")]
        public char TempUnit { get; internal set; }

        [JsonIgnore()]
        public float Temperature1 { get; internal set; }
    }
}
