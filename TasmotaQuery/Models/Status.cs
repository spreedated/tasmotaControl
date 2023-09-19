using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

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
        public ReadOnlyCollection<string> FriendlyNames { get; set; }

        [JsonProperty("DeviceName")]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime QueryTime { get; set; }
    }
}
