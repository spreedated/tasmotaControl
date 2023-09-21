using Microsoft.Maui.Storage;
using neXn.Lib.Maui.ViewLogic;
using neXn.Lib.Strings.Extensions;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace TasCon.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private string _AboutText;
        public string AboutText
        {
            get
            {
                return this._AboutText;
            }
            set
            {
                this._AboutText = value;
                base.OnPropertyChanged(nameof(this.AboutText));
            }
        }

        private string _NxLibraryVersion;
        public string NxLibraryVersion
        {
            get
            {
                return this._NxLibraryVersion;
            }
            set
            {
                this._NxLibraryVersion = value;
                base.OnPropertyChanged(nameof(this.NxLibraryVersion));
            }
        }

        private string _NxLibraryConfigVersion;
        public string NxLibraryConfigVersion
        {
            get
            {
                return this._NxLibraryConfigVersion;
            }
            set
            {
                this._NxLibraryConfigVersion = value;
                base.OnPropertyChanged(nameof(this.NxLibraryConfigVersion));
            }
        }

        private string _NxLibraryMauiVersion;
        public string NxLibraryMauiVersion
        {
            get
            {
                return this._NxLibraryMauiVersion;
            }
            set
            {
                this._NxLibraryMauiVersion = value;
                base.OnPropertyChanged(nameof(this.NxLibraryMauiVersion));
            }
        }

        private string _MauiText;
        public string MauiText
        {
            get
            {
                return this._MauiText;
            }
            set
            {
                this._MauiText = value;
                base.OnPropertyChanged(nameof(this.MauiText));
            }
        }

        public async Task LoadTexts()
        {
            if (!string.IsNullOrEmpty(this.AboutText))
            {
                return;
            }

            using (Stream stream = await FileSystem.OpenAppPackageFileAsync("AboutText.txt"))
            {
                using (StreamReader reader = new(stream))
                {
                    this.AboutText = reader.ReadToEnd();
                }
            }

            DateTime builddate = GetBuildDate(typeof(AboutViewModel).Assembly);

            Assembly a = this.GetType().Assembly;

            using (Stream stream = await FileSystem.OpenAppPackageFileAsync("AboutMauiText.txt"))
            {
                using (StreamReader reader = new(stream))
                {
                    this.MauiText = string.Format(reader.ReadToEnd(), a.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkDisplayName, builddate == default ? "N/A" : builddate, a.GetName().Version.ToNiceString());
                }
            }

            this.NxLibraryVersion = $"neXn.Lib {typeof(neXn.Lib.Strings.Common).Assembly.GetName().Version.ToNiceString()}";
            this.NxLibraryConfigVersion = $"neXn.Lib.ConfigurationHandler {typeof(neXn.Lib.ConfigurationHandler.Models.CongfigurationHandlerOptions).Assembly.GetName().Version.ToNiceString()}";
            this.NxLibraryMauiVersion = $"neXn.Lib.Maui {typeof(neXn.Lib.Maui.ViewElements.RefreshViewFix).Assembly.GetName().Version.ToNiceString()}";
        }

        private static DateTime GetBuildDate(Assembly assembly)
        {
            const string BuildVersionMetadataPrefix = "+build";

            AssemblyInformationalVersionAttribute attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                string value = attribute.InformationalVersion;
                int index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value[(index + BuildVersionMetadataPrefix.Length)..];
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        return result;
                    }
                }
            }
            return default;
        }
    }
}
