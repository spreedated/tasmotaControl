using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public class Device : IDevice
    {
        internal HttpMessageHandler httpHandler;
        public IPAddress Address { get; set; }
        public bool IsAvailable { get; internal set; }
        public DeviceProperties DeviceProperties { get; } = new();
        public DeviceStatusResponses DeviceStatusResponses { get; } = new();

        #region Constructor
        public Device(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address), "Address is null");
            }

            if (!IPAddress.TryParse(address, out IPAddress add) || address.Count(x => x == '.') != 3)
            {
                throw new ArgumentException("Invalid address", nameof(address));
            }

            this.Address = add;
        }

        public Device(string address, DeviceProperties deviceProperties) : this(address)
        {
            this.DeviceProperties = deviceProperties;
        }

        public Device(IPAddress address)
        {
            this.Address = address;
        }

        public Device(IPAddress address, DeviceProperties deviceProperties) : this(address)
        {
            this.DeviceProperties = deviceProperties;
        }
        #endregion
    }
}
