using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TasmotaQuery.Models;
using static TasmotaQuery.Constants;

namespace TasmotaQuery
{
    public class Device : IDevice
    {
        internal HttpMessageHandler httpHandler;

        [JsonProperty("address")]
        public IPAddress Address { get; set; }

        [JsonIgnore()]
        public bool IsAvailable { get; internal set; }

        [JsonProperty("deviceproperties")]
        public DeviceProperties DeviceProperties { get; } = new();

        [JsonProperty("devicestatusresponses")]
        public DeviceStatusResponses DeviceStatusResponses { get; } = new();

        [JsonIgnore()]
        public int ShutterPosition { get; set; }
        [JsonIgnore()]
        public bool ShutterRunning { get; set; }
        [JsonIgnore()]
        public int ShutterDirection { get; set; }
        public enum Powerstates
        {
            Unknown = 0,
            On,
            Off
        }
        [JsonIgnore()]
        public Powerstates Powerstate { get; set; }

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

        public IQuery Query()
        { 
            return new Query(this);
        }

        public async Task SetPower(bool on = true)
        {
            using (HttpClient hc = new())
            {
                await hc.GetAsync($"http://{this.Address}/cm?cmnd={(on ? POWER_ON : POWER_OFF)}");
            }

            await this.Query().GetStatus();
        }

        public async Task SetShutter(int position)
        {
            if (this.ShutterRunning)
            {
                return;
            }

            this.ShutterRunning = true;

            using (HttpClient hc = new())
            {
                await hc.GetAsync($"http://{this.Address}/cm?cmnd={SHUTTER_SET_POSITION}{position}");
            }

            this.ShutterRunning = false;
        }
    }
}
