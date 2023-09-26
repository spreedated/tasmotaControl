using System.IO;

namespace UnitTests
{
    internal static class HelperFunctions
    {
        internal static string LoadJson(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            using (Stream s = typeof(DeviceQueryTests).Assembly.GetManifestResourceStream($"{typeof(DeviceQueryTests).Assembly.GetName().Name}.JsonResponses.{name}{(!name.ToLower().EndsWith("json") ? ".json" : "")}"))
            {
                if (s == null)
                {
                    return null;
                }

                using (StreamReader sr = new(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
