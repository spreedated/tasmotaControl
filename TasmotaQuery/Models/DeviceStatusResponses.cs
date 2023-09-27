using Newtonsoft.Json;

namespace TasmotaQuery.Models
{
    public sealed class DeviceStatusResponses
    {
        [JsonProperty("state")]
        public State State { get; internal set; }
        [JsonProperty("status")]
        public Status Status { get; internal set; }
        [JsonProperty("firmware")]
        public Firmware Firmware { get; internal set; }
        [JsonProperty("time")]
        public Time Time { get; internal set; }
        [JsonProperty("sensors")]
        public Sensors Sensors { get; internal set; }
    }
}