using Newtonsoft.Json.Linq;
using System;

namespace TasmotaQuery.Json
{
    internal static class Extensions
    {
        internal static bool IsValidJson(this string json)
        {
            try
            {
                JObject.Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
