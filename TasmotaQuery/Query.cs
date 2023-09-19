using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TasmotaQuery.Models;

namespace TasmotaQuery
{
    public static class Query
    {
        public static async Task IsAvailable(this IDevice device)
        {
            using (HttpClient hc = new())
            {
                hc.Timeout = new TimeSpan(0, 0, 2);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm");
                    device.IsAvaiable = resp.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    ((Device)device).IsAvaiable = false;
                }
            }
        }

        public static async Task GetState(this IDevice device)
        {
            if (!device.IsAvaiable)
            {
                return;
            }

            using (HttpClient hc = new())
            {
                hc.Timeout = new TimeSpan(0, 0, 2);
                try
                {
                    HttpResponseMessage resp = await hc.GetAsync($"http://{device.Address}/cm?cmnd=state");
                    string json = await resp.Content.ReadAsStringAsync();

                    device.State = JsonConvert.DeserializeObject<State>(json);
                    device.State.QueryTime = DateTime.Now;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
