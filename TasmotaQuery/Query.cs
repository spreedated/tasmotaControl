using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public class Query : IQuery
    {
        public Device Device { get; internal set; }

        #region Constructor
        public Query(Device device)
        {
            this.Device = device;
        }
        #endregion

        private static async Task<HttpResponseMessage> GetResponseMessage(Device device, string url)
        {
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);

                return await hc.GetAsync(url);
            }
        }

        private static async Task<string> DownloadJsonResponse(Device device, string url)
        {
            return await (await GetResponseMessage(device, url)).Content.ReadAsStringAsync();
        }

        private static async Task<bool> IsJsonResponse(Device device, string url)
        {
            try
            {
                return (await GetResponseMessage(device, url)).IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<T> GetJsonResponseAndParseJsonAndDoMethod<T>(Device device, string url, Func<JObject, string> action) where T : StateBase
        {
            try
            {
                T state = JsonConvert.DeserializeObject<T>(action(JObject.Parse(await DownloadJsonResponse(device, url))));
                if (state == null)
                {
                    return default;
                }
                state.QueryTime = DateTime.Now;

                return state;
            }
            catch
            {
                return default;
            }
        }

        public async Task<IQuery> IsAvailable()
        {
            this.Device.IsAvailable = await IsJsonResponse(this.Device, $"http://{this.Device.Address}/cm");

            return this;
        }

        public async Task<IQuery> GetState()
        {
            this.Device.DeviceStatusResponses.State = await GetJsonResponseAndParseJsonAndDoMethod<State>(this.Device,
                $"http://{this.Device.Address}/cm?cmnd=state",
                x => x.ToString());

            if (this.Device.DeviceStatusResponses.State != null && this.Device.DeviceStatusResponses.State.Wifi != null)
            {
                this.Device.DeviceStatusResponses.State.Wifi.QueryTime = this.Device.DeviceStatusResponses.State.QueryTime;
            }

            return this;
        }

        public async Task<IQuery> GetStatus()
        {
            this.Device.DeviceStatusResponses.Status = await GetJsonResponseAndParseJsonAndDoMethod<Status>(this.Device,
                $"http://{this.Device.Address}/cm?cmnd=status",
                x => x["Status"].ToString());

            return this;
        }

        /// <summary>
        /// Retrieves 'Status 2' Information<br/>
        /// Firmware
        /// </summary>
        /// <returns></returns>
        public async Task<IQuery> GetStatus2Firmware()
        {
            this.Device.DeviceStatusResponses.Firmware = await GetJsonResponseAndParseJsonAndDoMethod<Firmware>(this.Device,
                $"http://{this.Device.Address}/cm?cmnd=status%202",
                x => x["StatusFWR"].ToString());

            return this;
        }

        /// <summary>
        /// Retrieves 'Status 7' Information<br/>
        /// Time
        /// </summary>
        /// <returns></returns>
        public async Task<IQuery> GetStatus7Time()
        {
            this.Device.DeviceStatusResponses.Time = await GetJsonResponseAndParseJsonAndDoMethod<Time>(this.Device,
                $"http://{this.Device.Address}/cm?cmnd=status%207",
                x => x["StatusTIM"].ToString());

            return this;
        }

        /// <summary>
        /// Retrieves 'Status 10' Information<br/>
        /// Sensors
        /// </summary>
        /// <returns></returns>
        public async Task<IQuery> GetStatus10Sensors()
        {
            float temp1 = 0.0f;

            this.Device.DeviceStatusResponses.Sensors = await GetJsonResponseAndParseJsonAndDoMethod<Sensors>(this.Device,
                $"http://{this.Device.Address}/cm?cmnd=status%2010",
                (x) =>
                {
                    temp1 = x.SelectToken("StatusSNS.ANALOG.Temperature1", false).ToObject<float>();
                    return x["StatusSNS"].ToString();
                });

            if (this.Device.DeviceStatusResponses.Sensors != null)
            {
                this.Device.DeviceStatusResponses.Sensors.Temperature1 = temp1;
            }

            return this;
        }
    }
}