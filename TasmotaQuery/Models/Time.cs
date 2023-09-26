using Newtonsoft.Json;
using System;
using TasmotaQuery.Json;

namespace TasmotaQuery.Models
{
    public sealed class Time
    {
        [JsonProperty("UTC")]
        public DateTime Utc { get; set; }

        [JsonProperty("Local")]
        public DateTime Local { get; set; }

        [JsonProperty("StartDST")]
        public DateTime StartDst { get; set; }

        [JsonProperty("EndDST")]
        public DateTime EndDst { get; set; }

        [JsonProperty("Timezone")]
        public int Timezone { get; set; }

        [JsonProperty("Sunrise")]
        [JsonConverter(typeof(TimeOnlyTasmotaJsonConverter))]
        public TimeOnly Sunrise { get; set; }

        [JsonProperty("Sunset")]
        [JsonConverter(typeof(TimeOnlyTasmotaJsonConverter))]
        public TimeOnly Sunset { get; set; }

        [JsonIgnore()]
        public DateTime QueryTime { get; set; }
    }
}
