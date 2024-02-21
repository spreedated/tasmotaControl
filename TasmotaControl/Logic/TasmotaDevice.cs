using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TasmotaQuery;
using TasmotaQuery.Models;

namespace TasCon.Logic
{
    public sealed class TasmotaDevice : Device
    {
        [JsonProperty("viewPriority")]
        public short ViewPriority { get; set; }

        [JsonIgnore()]
        public Exception Failure { get; set; }

        #region Constructor
        public TasmotaDevice(string address) : base(address)
        {
        }

        public TasmotaDevice(string address, DeviceProperties deviceProperties) : base(address, deviceProperties)
        {
        }

        public TasmotaDevice(IPAddress address) : base(address)
        {
        }

        public TasmotaDevice(IPAddress address, DeviceProperties deviceProperties) : base(address, deviceProperties)
        {
        }
        #endregion

        public async Task RefreshStatus()
        {
            await base.Query().GetStatus();

            try
            {
                using (HttpClient hc = new())
                {
                    hc.Timeout = new TimeSpan(0, 0, 10);

                    if (base.DeviceProperties.IsShutter)
                    {
                        HttpResponseMessage webResponse = await hc.GetAsync($"http://{this.Address}/cm?cmnd=ShutterPosition1");
                        string json = await webResponse.Content.ReadAsStringAsync();

                        JObject shutterPosition = JObject.Parse(json);

                        if (shutterPosition != null && shutterPosition.HasValues)
                        {
                            base.ShutterPosition = shutterPosition["Shutter1"]["Position"].Value<int>();
                            base.ShutterDirection = shutterPosition["Shutter1"]["Direction"].Value<int>();
                        }
                    }

                    if (base.DeviceProperties.HasTemperaturesensor)
                    {
                        await this.Query().GetStatus10Sensors();
                    }
                }

                this.Failure = null;
            }
            catch (Exception ex)
            {
                this.Failure = new TimeoutException("Device unavailable", ex);
            }
        }
    }
}
