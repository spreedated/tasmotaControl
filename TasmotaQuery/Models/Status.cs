using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public sealed record Status : StateBase
    {
        [JsonProperty("PowerOnState")]
        public short PowerOnState { get; internal set; }

        [JsonProperty("LedState")]
        public short LedState { get; internal set; }

        [JsonProperty("Topic")]
        public string Topic { get; internal set; }

        [JsonProperty("FriendlyName")]
        public string[] FriendlyNames { get; internal set; }

        [JsonProperty("DeviceName")]
        public string Name { get; internal set; }
    }
}
