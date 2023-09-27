using Newtonsoft.Json;
using System;

namespace TasmotaQuery.Models
{
    public abstract record StateBase
    {
        [JsonIgnore()]
        public DateTime QueryTime { get; internal set; }
    }
}