using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public static class Queries
    {
        public static async Task<IDevice> IsAvailable(this Device device)
        {
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm");
                    device.IsAvaiable = resp.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    device.IsAvaiable = false;
                }

                return device;
            }
        }

        public static async Task<IDevice> GetState(this Device device)
        {
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm?cmnd=state");
                    string json = await resp.Content.ReadAsStringAsync();

                    device.State = JsonConvert.DeserializeObject<State>(json);
                    device.State.QueryTime = DateTime.Now;
                    if (device.State.Wifi != null)
                    {
                        device.State.Wifi.QueryTime = device.State.QueryTime;
                    }
                }
                catch (Exception)
                {
                    device.State = null;
                }
            }

            return device;
        }

        public static async Task<IDevice> GetStatus(this Device device)
        {
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm?cmnd=status");
                    string json = await resp.Content.ReadAsStringAsync();

                    JObject jo = JObject.Parse(json);

                    device.Status = JsonConvert.DeserializeObject<Status>(jo["Status"].ToString());
                    device.Status.QueryTime = DateTime.Now;
                    
                }
                catch (Exception)
                {
                    device.Status = null;
                }
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
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm?cmnd=status%202");
                    string json = await resp.Content.ReadAsStringAsync();

                    JObject jo = JObject.Parse(json);

                    device.Firmware = JsonConvert.DeserializeObject<Firmware>(jo["StatusFWR"].ToString());
                    device.Firmware.QueryTime = DateTime.Now;

                }
                catch (Exception)
                {
                    device.Firmware = null;
                }
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
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm?cmnd=status%207");
                    string json = await resp.Content.ReadAsStringAsync();

                    JObject jo = JObject.Parse(json);

                    device.Time = JsonConvert.DeserializeObject<Time>(jo["StatusTIM"].ToString());
                    device.Time.QueryTime = DateTime.Now;

                }
                catch (Exception)
                {
                    device.Time = null;
                }
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
            using (HttpClient hc = device.httpHandler != null ? new(device.httpHandler) : new())
            {
                hc.Timeout = new TimeSpan(0, 0, 10);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm?cmnd=status%2010");
                    string json = await resp.Content.ReadAsStringAsync();

                    JObject jo = JObject.Parse(json);

                    device.Sensors = JsonConvert.DeserializeObject<Sensors>(jo["StatusSNS"].ToString());
                    device.Sensors.Temperature1 = jo.SelectToken("StatusSNS.ANALOG.Temperature1", false).ToObject<float>();
                    device.Sensors.QueryTime = DateTime.Now;

                }
                catch (Exception)
                {
                    device.Time = null;
                }
            }

            return device;
        }
    }
}
