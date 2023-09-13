using Microsoft.Maui.Controls;
using neXn.Lib.Maui.ViewLogic;

namespace TasCon.ViewModels
{
    public class DebugViewModel : ViewModelBase
    {
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

        private string _DebugText;
        public string DebugText
        {
            get
            {
                return this._DebugText;
            }
            set
            {
                this._DebugText = value;
                base.OnPropertyChanged(nameof(this.DebugText));
            }
        }
    }
}
