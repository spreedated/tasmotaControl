using Newtonsoft.Json;
using System;
using TasmotaQuery.Json;

namespace TasmotaQuery.Models
{
    public sealed record Time
    {
        [JsonProperty("UTC")]
        public DateTime Utc { get; internal set; }

        [JsonProperty("Local")]
        public DateTime Local { get; internal set; }

        [JsonProperty("StartDST")]
        public DateTime StartDst { get; internal set; }

        [JsonProperty("EndDST")]
        public DateTime EndDst { get; internal set; }

        [JsonProperty("Timezone")]
        public int Timezone { get; internal set; }

        [JsonProperty("Sunrise")]
        [JsonConverter(typeof(TimeOnlyTasmotaJsonConverter))]
        public TimeOnly Sunrise { get; internal set; }

        [JsonProperty("Sunset")]
        [JsonConverter(typeof(TimeOnlyTasmotaJsonConverter))]
        public TimeOnly Sunset { get; internal set; }

        [JsonIgnore()]
        public DateTime QueryTime { get; internal set; }
    }
}
