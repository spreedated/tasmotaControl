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
        public IPAddress Address { get; set; }
        public bool IsAvaiable { get; set; }

        public State State { get; set; }

        #region Constructor
        public Device(string address)
        {
            if (!IPAddress.TryParse(address, out IPAddress add))
            {
                throw new ArgumentException("Invalid address", nameof(address));
            }

            this.Address = add;
        }

        public Device(IPAddress address)
        {
            this.Address = address;
        }
        #endregion
    }
}
