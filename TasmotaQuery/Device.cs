using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public class Device : IDevice
    {
        internal HttpMessageHandler httpHandler;
        public IPAddress Address { get; set; }
        public bool IsAvaiable { get; set; }
        public DeviceProperties DeviceProperties { get; } = new();
        public State State { get; set; }
        public Status Status { get; set; }
        public Firmware Firmware { get; set; }

        #region Constructor
        public Device(string address)
        {
            if (!IPAddress.TryParse(address, out IPAddress add))
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
