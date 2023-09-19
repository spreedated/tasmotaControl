using System.Net;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public interface IDevice
    {
        public IPAddress Address { get; set; }
        public bool IsAvaiable { get; set; }
        public State State { get; set; }
    }
}
