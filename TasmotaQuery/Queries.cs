using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public static class Queries
    {
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

        public static async Task<IDevice> IsAvailable(this Device device)
        {
            device.IsAvailable = await IsJsonResponse(device, $"http://{device.Address}/cm");

            return device;
        }

        public static async Task<IDevice> GetState(this Device device)
        {
            device.DeviceStatusResponses.State = await GetJsonResponseAndParseJsonAndDoMethod<State>(device,
                $"http://{device.Address}/cm?cmnd=state",
                x => x.ToString());

            if (device.DeviceStatusResponses.State != null && device.DeviceStatusResponses.State.Wifi != null)
            {
                device.DeviceStatusResponses.State.Wifi.QueryTime = device.DeviceStatusResponses.State.QueryTime;
            }

            return device;
        }

        public static async Task<IDevice> GetStatus(this Device device)
        {
            device.DeviceStatusResponses.Status = await GetJsonResponseAndParseJsonAndDoMethod<Status>(device,
                $"http://{device.Address}/cm?cmnd=status",
                x => x["Status"].ToString());

            return device;
        }

        /// <summary>
        /// Retrieves 'Status 2' Information<br/>
        /// Firmware
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static async Task<IDevice> GetStatus2Firmware(this Device device)
        {
            device.DeviceStatusResponses.Firmware = await GetJsonResponseAndParseJsonAndDoMethod<Firmware>(device,
                $"http://{device.Address}/cm?cmnd=status%202",
                x => x["StatusFWR"].ToString());

            return device;
        }

        /// <summary>
        /// Retrieves 'Status 7' Information<br/>
        /// Time
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static async Task<IDevice> GetStatus7Time(this Device device)
        {
            device.DeviceStatusResponses.Time = await GetJsonResponseAndParseJsonAndDoMethod<Time>(device,
                $"http://{device.Address}/cm?cmnd=status%207",
                x => x["StatusTIM"].ToString());

            return device;
        }

        /// <summary>
        /// Retrieves 'Status 10' Information<br/>
        /// Sensors
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static async Task<IDevice> GetStatus10Sensors(this Device device)
        {
            float temp1 = 0.0f;

            device.DeviceStatusResponses.Sensors = await GetJsonResponseAndParseJsonAndDoMethod<Sensors>(device,
                $"http://{device.Address}/cm?cmnd=status%2010",
                (x) =>
                    {
                        temp1 = x.SelectToken("StatusSNS.ANALOG.Temperature1", false).ToObject<float>();
                        return x["StatusSNS"].ToString();
                    });

            if (device.DeviceStatusResponses.Sensors != null)
            {
                device.DeviceStatusResponses.Sensors.Temperature1 = temp1;
            }

            return device;
        }
    }
}
