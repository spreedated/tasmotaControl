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
            return (await GetResponseMessage(device, url)).IsSuccessStatusCode;
        }

        public static async Task<IDevice> IsAvailable(this Device device)
        {
            try
            {
                device.IsAvailable = await IsJsonResponse(device, $"http://{device.Address}/cm");
            }
            catch (Exception)
            {
                device.IsAvailable = false;
            }

            return device;
        }

        public static async Task<IDevice> GetState(this Device device)
        {
            try
            {
                device.DeviceStatusResponses.State = JsonConvert.DeserializeObject<State>(await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=state"));
                device.DeviceStatusResponses.State.QueryTime = DateTime.Now;

                if (device.DeviceStatusResponses.State.Wifi != null)
                {
                    device.DeviceStatusResponses.State.Wifi.QueryTime = device.DeviceStatusResponses.State.QueryTime;
                }
            }
            catch (Exception)
            {
                device.DeviceStatusResponses.State = null;
            }

            return device;
        }

        public static async Task<IDevice> GetStatus(this Device device)
        {
            try
            {
                JObject jo = JObject.Parse(await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status"));

                device.DeviceStatusResponses.Status = JsonConvert.DeserializeObject<Status>(jo["Status"].ToString());
                device.DeviceStatusResponses.Status.QueryTime = DateTime.Now;

            }
            catch (Exception)
            {
                device.DeviceStatusResponses.Status = null;
            }

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
            try
            {
                JObject jo = JObject.Parse(await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status%202"));

                device.DeviceStatusResponses.Firmware = JsonConvert.DeserializeObject<Firmware>(jo["StatusFWR"].ToString());
                device.DeviceStatusResponses.Firmware.QueryTime = DateTime.Now;

            }
            catch (Exception)
            {
                device.DeviceStatusResponses.Firmware = null;
            }

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
            try
            {
                JObject jo = JObject.Parse(await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status%207"));

                device.DeviceStatusResponses.Time = JsonConvert.DeserializeObject<Time>(jo["StatusTIM"].ToString());
                device.DeviceStatusResponses.Time.QueryTime = DateTime.Now;

            }
            catch (Exception)
            {
                device.DeviceStatusResponses.Time = null;
            }

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
            try
            {
                JObject jo = JObject.Parse(await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status%2010"));

                device.DeviceStatusResponses.Sensors = JsonConvert.DeserializeObject<Sensors>(jo["StatusSNS"].ToString());
                device.DeviceStatusResponses.Sensors.Temperature1 = jo.SelectToken("StatusSNS.ANALOG.Temperature1", false).ToObject<float>();
                device.DeviceStatusResponses.Sensors.QueryTime = DateTime.Now;

            }
            catch (Exception)
            {
                device.DeviceStatusResponses.Sensors = null;
            }

            return device;
        }
    }
}
