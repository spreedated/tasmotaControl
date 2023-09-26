using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public sealed class Firmware
    {
        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("BuildDateTime")]
        public DateTime BuildDateTime { get; set; }

        [JsonProperty("Core")]
        public string Core { get; set; }

        [JsonProperty("SDK")]
        public string Sdk { get; set; }

        [JsonProperty("CpuFrequency")]
        public int CpuFrequency { get; set; }

        [JsonProperty("Hardware")]
        public string Hardware { get; set; }

        [JsonProperty("CR")]
        public string Clockrate { get; set; }

        [JsonIgnore()]
        public DateTime QueryTime { get; set; }
    }
}
