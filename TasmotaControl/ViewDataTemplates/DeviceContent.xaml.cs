using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using TasCon.Logic;

namespace TasCon.ViewDataTemplates;

public partial class DeviceContent : ContentView
{
    private System.Timers.Timer autoRefreshTimer;
    internal Animation RefreshAnimation;
    public bool Initializing { get; private set; } = true;

    public static readonly BindableProperty DeviceProperty = BindableProperty.CreateAttached(nameof(Device), typeof(TasmotaDevice), typeof(DeviceContent), null);

    public TasmotaDevice Device
    {
        get => (TasmotaDevice)base.GetValue(DeviceProperty);
        set => base.SetValue(DeviceProperty, value);
    }

    public ICommand ToggleCommand { get; } = new Command<ContentView>((cv) =>
    {
        Task.Factory.StartNew(async () =>
        {
            if (((DeviceContent)cv).Device.Powerstate == TasmotaDevice.Powerstates.On)
            {
                await ((DeviceContent)cv).Device.SetPower(false);
            }
            else
            {
                await ((DeviceContent)cv).Device.SetPower();
            }

            ((DeviceContent)cv).Dispatcher.Dispatch(async () =>
            {
                await ((DeviceContent)cv).DeviceRefresh();
            });
        });
    });

    public ICommand ShutterSliderDragCompletedCommand { get; } = new Command<ContentView>((cv) =>
    {
        if (((DeviceContent)cv).Initializing)
        {
            return;
        }

        Task.Factory.StartNew(async () =>
        {
            await ((DeviceContent)cv).Device.SetShutter(((DeviceContent)cv).ShutterValue);

            ((DeviceContent)cv).Dispatcher.Dispatch(async () =>
            {
                await ((DeviceContent)cv).DeviceRefresh();
            });
        });
    });

    private ContentView _Instance;
    public ContentView Instance
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

    private int _ShutterValue;
    public int ShutterValue
    {
        get
        {
            return this._ShutterValue;
        }
        set
        {
            this._ShutterValue = value;
            base.OnPropertyChanged(nameof(this.ShutterValue));
        }
    }

    private bool _IsLoading;
    public bool IsLoading
    {
        get
        {
            return this._IsLoading;
        }
        set
        {
            this._IsLoading = value;
            base.OnPropertyChanged(nameof(this.IsLoading));
            base.OnPropertyChanged(nameof(this.IsFirstLoading));
        }
    }

    private bool _IsFirstLoad = true;
    public bool IsFirstLoad
    {
        get
        {
            return this._IsFirstLoad;
        }
        set
        {
            this._IsFirstLoad = value;
            base.OnPropertyChanged(nameof(this.IsFirstLoad));
        }
    }

    public bool IsFirstLoading
    {
        get
        {
            return this.IsFirstLoad && this.IsLoading;
        }
    }

    private bool _HasError;
    public bool HasError
    {
        get
        {
            return this._HasError;
        }
        set
        {
            this._HasError = value;
            base.OnPropertyChanged(nameof(this.HasError));
            base.OnPropertyChanged(nameof(this.IsFirstLoading));
        }
    }

    private string _ErrorMessage;
    public string ErrorMessage
    {
        get
        {
            return this._ErrorMessage;
        }
        set
        {
            this._ErrorMessage = value;
            base.OnPropertyChanged(nameof(this.ErrorMessage));
        }
    }

    private string _LoadingText;
    public string LoadingText
    {
        get
        {
            return this._LoadingText;
        }
        set
        {
            this._LoadingText = value;
            base.OnPropertyChanged(nameof(this.LoadingText));
        }
    }

    private bool _IsExpandedInformation;
    public bool IsExpandedInformation
    {
        get
        {
            return this._IsExpandedInformation;
        }
        set
        {
            this._IsExpandedInformation = value;
            base.OnPropertyChanged(nameof(this.IsExpandedInformation));
        }
    }

    public DeviceContent()
    {
        this.InitializeComponent();
        this.Instance = this;

        this.FirstLoadingAnimation();
        this.InitializeRotationAnimation();
        this.InitializeAutoRefreshTimer();
    }

    private void InitializeRotationAnimation()
    {
        this.RefreshAnimation = new((v) => this.RefreshIcon.Rotation = v, 0, 360, Easing.Linear);
        this.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(this.IsLoading))
            {
                if (this.IsLoading)
                {
                    this.RefreshAnimation.Commit(this, "refreshRotation", 16, 1000, Easing.Linear, (v, c) => this.RefreshIcon.Rotation = 0, () => true);
                }
                else
                {
                    this.AbortAnimation("refreshRotation");
                }
            }
        };
    }

    private void InitializeAutoRefreshTimer()
    {
        this.autoRefreshTimer = new()
        {
            Enabled = true,
            Interval = new TimeSpan(0, 1, 0).TotalMilliseconds
        };
        this.autoRefreshTimer.Elapsed += this.AutoRefreshTimer_Tick;

        this.autoRefreshTimer.Start();
    }

    private void FirstLoadingAnimation()
    {
        Task.Factory.StartNew(() =>
        {
            Thread.CurrentThread.Name = $"LoadingAnimation";
            int dots = 3;

            void DotAnimation(string prefix = null, string suffix = null)
            {
                while (this.IsFirstLoad)
                {
                    this.LoadingText = $"{prefix}{new string('.', dots)}{suffix}";
                    dots++;

                    if (dots >= 4)
                    {
                        dots = 1;
                    }

                    Thread.Sleep(200);

                    DotAnimation(prefix, suffix);
                }
            }

            DotAnimation("Loading ");
        });
    }

    private void AutoRefreshTimer_Tick(object sender, ElapsedEventArgs e)
    {
        if (this.IsLoading)
        {
            return;
        }

        Task.Factory.StartNew(async () =>
        {
            await this.DeviceRefresh();
        });
    }

    public async Task DeviceRefresh()
    {
        if (this.IsLoading)
        {
            return;
        }

        this.IsLoading = true;

        await this.Device.RefreshStatus();

        if (this.Device.Failure != null)
        {
            this.HasError = true;
            this.ErrorMessage = this.Device.Failure.Message;
            this.IsFirstLoad = false;
            this.IsLoading = false;

            return;
        }

        this.HasError = false;
        this.ErrorMessage = null;

        await this.Device.RefreshStatus2();

        this.Dispatcher.Dispatch(() =>
        {
            this.OnPropertyChanged(nameof(this.Device));

            if (this.Device.IsShutter && this.Device.ShutterDirection == 0)
            {
                this._ShutterValue = this.Device.ShutterPosition;
                base.OnPropertyChanged(nameof(this.ShutterValue));
                this.Initializing = false;
            }
        });

        this.IsLoading = false;
        this.IsFirstLoad = false;
    }

    private void Me_Loaded(object sender, EventArgs e)
    {
        if (this.IsLoading)
        {
            return;
        }

        Task.Factory.StartNew(async () =>
        {
            await this.DeviceRefresh();
        });
    }
}