using Microsoft.Maui.Controls;
using neXn.Lib.Maui.ViewLogic;
using System.Windows.Input;
using TasCon.Views;
using TasCon.Logic;
using System.Collections.ObjectModel;

namespace TasCon.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ICommand RefreshCommand { get; } = new Command<ContentPage>(async (c) =>
        {
            await ((MainPage)c).RefreshDevices();
            ((MainPageViewModel)c.BindingContext).IsRefreshing = false;
        });

        private ObservableCollection<TasmotaDevice> _ThisDevices;
        public ObservableCollection<TasmotaDevice> ThisDevices
        {
            get
            {
                return this._ThisDevices;
            }
            set
            {
                this._ThisDevices = value;
                base.OnPropertyChanged(nameof(this.ThisDevices));
            }
        }

        private ContentPage _Instance;
        public ContentPage Instance
        {
            get
            {
                return this._Instance;
            }
            set
            {
                this._Instance = value;
                base.OnPropertyChanged(nameof(this.Instance));
            }
        }

        private bool _IsRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return this._IsRefreshing;
            }
            set
            {
                this._IsRefreshing = value;
                base.OnPropertyChanged(nameof(this.IsRefreshing));
            }
        }

        private bool _NoPermission;
        public bool NoPermission
        {
            get
            {
                return this._NoPermission;
            }
            set
            {
                this._NoPermission = value;
                base.OnPropertyChanged(nameof(this.NoPermission));
            }
        }

        private bool _IsCorrectWifi;
        public bool IsCorrectWifi
        {
            get
            {
                return this._IsCorrectWifi;
            }
            set
            {
                this._IsCorrectWifi = value;
                base.OnPropertyChanged(nameof(this.IsCorrectWifi));
            }
        }

        private string _WiFiSSID;
        public string WiFiSSID
        {
            get
            {
                return this._WiFiSSID;
            }
            set
            {
                this._WiFiSSID = value;
                base.OnPropertyChanged(nameof(this.WiFiSSID));
            }
        }
    }
}
