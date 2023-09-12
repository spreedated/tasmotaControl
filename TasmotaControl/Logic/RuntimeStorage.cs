using Microsoft.Maui.Controls;
using neXn.Lib.ConfigurationHandler;
using System.Collections.Generic;

namespace TasCon.Logic
{
    internal static class RuntimeStorage
    {
        public static ConfigurationHandler<Models.Configuration> ConfigurationHandler { get; set; }
        public static List<Page> PreloadedPages { get; } = new();
    }
}