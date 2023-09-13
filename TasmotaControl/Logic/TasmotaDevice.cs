using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static TasCon.Logic.Constants;

namespace TasCon.Logic
{
    public sealed class TasmotaDevice
    {
        public string Address { get; private set; }
        public string DeviceName { get; private set; }
        public string FriendlyName { get; private set; }
        public string Firmware { get; private set; }
        public string Hardware { get; private set; }
        [JsonIgnore()]
        public float AnalogTemperature { get; private set; }
        [JsonIgnore()]
        public Powerstates Powerstate { get; private set; }
        [JsonIgnore()]
        public DateTime Refreshed { get; private set; }
        public bool IsShutter { get; set; }
        public bool HasTemperature { get; set; }
        [JsonIgnore()]
        public Exception Failure { get; set; }
        [JsonIgnore()]
        public int ShutterPosition { get; private set; }
        [JsonIgnore()]
        public bool ShutterRunning { get; private set; }
        [JsonIgnore()]
        public int ShutterDirection { get; private set; }
        [JsonIgnore()]
        public DateTime Sunrise { get; private set; }
        [JsonIgnore()]
        public DateTime Sunset { get; private set; }
        [JsonIgnore()]
        public DateTime Localtime { get; private set; }
        public short ViewPriority { get; set; }

        public enum Powerstates
        {
            Unknown = 0,
            On,
            Off
        }

        public TasmotaDevice(string address)
        {
            this.Address = address;
        }

        public void ChangeAddress(short lastOctett)
        {
            this.Address = this.Address.Substring(0, this.Address.LastIndexOf('.') + 1) + lastOctett.ToString();
        }

        public async Task SetPower(bool on = true)
        {
            using (HttpClient hc = new())
            {
                await hc.GetAsync($"http://{this.Address}/cm?cmnd={(on ? POWER_ON : POWER_OFF)}");
            }

            await this.RefreshStatus();
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

        public async Task RefreshStatus()
        {
            try
            {
                JObject jo = null;

                using (HttpClient hc = new())
                {
                    hc.Timeout = new TimeSpan(0, 0, 10);

                    HttpResponseMessage webResponse = await hc.GetAsync($"http://{this.Address}/cm?cmnd=status");
                    string json = await webResponse.Content.ReadAsStringAsync();

                    jo = JObject.Parse(json);

                    if (this.IsShutter)
                    {
                        webResponse = await hc.GetAsync($"http://{this.Address}/cm?cmnd=ShutterPosition1");
                        json = await webResponse.Content.ReadAsStringAsync();

                        JObject shutterPosition = JObject.Parse(json);

                        if (shutterPosition != null && shutterPosition.HasValues)
                        {
                            this.ShutterPosition = shutterPosition["Shutter1"]["Position"].Value<int>();
                            this.ShutterDirection = shutterPosition["Shutter1"]["Direction"].Value<int>();
                        }
                    }

                    if (this.HasTemperature)
                    {
                        await this.RefreshStatus10();
                    }
                }

                if (jo == null)
                {
                    throw new InvalidOperationException("No content received");
                }

                string[] fNames = jo["Status"]["FriendlyName"]?.ToObject<string[]>();

                if (fNames != null && fNames.Any())
                {
                    this.FriendlyName = fNames[0];
                }

                this.DeviceName = jo["Status"]["DeviceName"].Value<string>();
                this.Powerstate = jo["Status"]["Power"].Value<bool>() ? Powerstates.On : Powerstates.Off;

                this.Refreshed = DateTime.Now;
                this.Failure = null;
            }
            catch (Exception ex)
            {
                this.Failure = new TimeoutException("Device unavailable", ex);
            }
        }

        /// <summary>
        /// Retrieves 'Status 2' Information<br/>
        /// Firmware
        /// </summary>
        /// <returns></returns>
        public async Task RefreshStatus2()
        {
            JObject jo = null;

            using (HttpClient hc = new())
            {
                HttpResponseMessage webResponse = await hc.GetAsync($"http://{this.Address}/cm?cmnd=status%202");
                string json = await webResponse.Content.ReadAsStringAsync();

                jo = JObject.Parse(json);
            }

            this.Firmware = jo["StatusFWR"]["Version"]?.ToObject<string>();
            this.Hardware = jo["StatusFWR"]["Hardware"]?.ToObject<string>();
        }

        /// <summary>
        /// Retrieves 'Status 7' Information<br/>
        /// Time
        /// </summary>
        /// <returns></returns>
        public async Task RefreshStatus7()
        {
            JObject jo = null;

            using (HttpClient hc = new())
            {
                HttpResponseMessage webResponse = await hc.GetAsync($"http://{this.Address}/cm?cmnd=status%207");
                string json = await webResponse.Content.ReadAsStringAsync();

                jo = JObject.Parse(json);
            }

            this.Sunrise = jo["StatusTIM"]["Sunrise"].ToObject<DateTime>();
            this.Sunset = jo["StatusTIM"]["Sunset"].ToObject<DateTime>();
            this.Localtime = jo["StatusTIM"]["Local"].ToObject<DateTime>();
        }

        /// <summary>
        /// Retrieves 'Status 10' Information<br/>
        /// Sensors
        /// </summary>
        /// <returns></returns>
        public async Task RefreshStatus10()
        {
            JObject jo = null;

            using (HttpClient hc = new())
            {
                HttpResponseMessage webResponse = await hc.GetAsync($"http://{this.Address}/cm?cmnd=status%2010");
                string json = await webResponse.Content.ReadAsStringAsync();

                jo = JObject.Parse(json);
            }

            JToken temp = jo.SelectToken("StatusSNS.ANALOG.Temperature1", false);

            if (temp != null)
            {
                this.AnalogTemperature = temp.ToObject<float>();
            }
        }
    }
}
