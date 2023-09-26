using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public sealed class Status
    {
        [JsonProperty("PowerOnState")]
        public short PowerOnState { get; set; }

        [JsonProperty("LedState")]
        public short LedState { get; set; }

        [JsonProperty("Topic")]
        public string Topic { get; set; }

        [JsonProperty("FriendlyName")]
        public string[] FriendlyNames { get; set; }

        [JsonProperty("DeviceName")]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime QueryTime { get; set; }
    }
}
