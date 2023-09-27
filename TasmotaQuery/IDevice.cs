using System.Net;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public interface IDevice
    {
        public IPAddress Address { get; }
        public bool IsAvailable { get; }
        public DeviceProperties DeviceProperties { get; }
        public DeviceStatusResponses DeviceStatusResponses { get; }
    }
}
