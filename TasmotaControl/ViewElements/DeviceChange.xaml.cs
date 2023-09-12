using Microsoft.Maui.Controls;
using TasCon.Logic;

namespace TasCon.ViewElements;

public partial class DeviceChange : ContentView
{
    private TasmotaDevice _Device;
    public TasmotaDevice Device
    {
        get
        {
            return this._Device;
        }
        set
        {
            this._Device = value;
            base.OnPropertyChanged(nameof(this.Device));

            if (value != null && !string.IsNullOrEmpty(value.Address))
            {
                this.LastOctett = short.Parse(value.Address[(value.Address.LastIndexOf('.') + 1)..]);
            }
        }
    }

    private short _LastOctett;
    public short LastOctett
    {
        get
        {
            return this._LastOctett;
        }
        set
        {
            this._LastOctett = value;
            base.OnPropertyChanged(nameof(this.LastOctett));
        }
    }

    public DeviceChange()
    {
        this.InitializeComponent();
        this.BindingContext = this;
    }

    private bool IsChangingText = false;

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!this.IsChangingText)
        {
            this.IsChangingText = true;
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                this.IsChangingText = false;
                return;
            }
            if (short.TryParse(e.NewTextValue, out short loctett) && loctett <= 255)
            {
                ((Entry)sender).Text = loctett.ToString();
                this.Device.ChangeAddress(loctett);
            }
            else
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
            this.IsChangingText = false;
        }

    }
}