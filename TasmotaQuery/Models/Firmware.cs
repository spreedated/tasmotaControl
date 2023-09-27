using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public sealed record Firmware
    {
        [JsonProperty("Version")]
        public string Version { get; internal set; }

        [JsonProperty("BuildDateTime")]
        public DateTime BuildDateTime { get; internal set; }

        [JsonProperty("Core")]
        public string Core { get; internal set; }

        [JsonProperty("SDK")]
        public string Sdk { get; internal set; }

        [JsonProperty("CpuFrequency")]
        public int CpuFrequency { get; internal set; }

        [JsonProperty("Hardware")]
        public string Hardware { get; internal set; }

        [JsonProperty("CR")]
        public string Clockrate { get; internal set; }
        
        [JsonIgnore()]
        public DateTime QueryTime { get; internal set; }
    }
}
