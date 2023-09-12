using Newtonsoft.Json;
using System.Collections.Generic;
using TasCon.Logic;

namespace TasCon.Models
{
    public sealed class Configuration
    {
        [JsonProperty("devicelist")]
        public List<TasmotaDevice> Devices { get; set; } = new();
    }
}
