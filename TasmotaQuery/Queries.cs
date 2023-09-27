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
        private static async Task<string> DownloadJsonResponse(Device device, string url)
        {
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);

                HttpResponseMessage resp = await hc.GetAsync(url);
                return await resp.Content.ReadAsStringAsync();
            }
        }

        public static async Task<IDevice> IsAvailable(this Device device)
        {
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm");
                    device.IsAvailable = resp.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    device.IsAvailable = false;
                }

                return device;
            }
        }

        public static async Task<IDevice> GetState(this Device device)
        {
            try
            {
                string json = await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=state");
                    
                device.DeviceStatusResponses.State = JsonConvert.DeserializeObject<State>(json);
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
                string json = await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status");

                JObject jo = JObject.Parse(json);

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
                string json = await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status%202");

                JObject jo = JObject.Parse(json);

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
                string json = await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status%207");

                JObject jo = JObject.Parse(json);

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
                string json = await DownloadJsonResponse(device, $"http://{device.Address}/cm?cmnd=status%2010");

                JObject jo = JObject.Parse(json);

                device.DeviceStatusResponses.Sensors = JsonConvert.DeserializeObject<Sensors>(jo["StatusSNS"].ToString());
                device.DeviceStatusResponses.Sensors.Temperature1 = jo.SelectToken("StatusSNS.ANALOG.Temperature1", false).ToObject<float>();
                device.DeviceStatusResponses.Sensors.QueryTime = DateTime.Now;

            }
            catch (Exception)
            {
                device.DeviceStatusResponses.Time = null;
            }

            return device;
        }
    }
}
