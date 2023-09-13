using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;
using TasCon.ViewModels;

namespace TasCon.Views;

public partial class Debug : ContentPage
{
	public Debug()
	{
		this.InitializeComponent();
        ((DebugViewModel)this.BindingContext).Instance = this;

        using (FileStream fs = File.Open(Path.Combine(FileSystem.Current.AppDataDirectory, "config.json"), FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (StreamReader r = new(fs))
            {
                ((DebugViewModel)this.BindingContext).DebugText = r.ReadToEnd();
            }
        }
	}
}