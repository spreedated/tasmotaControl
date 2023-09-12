using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using TasCon.ViewModels;

namespace TasCon.Views;

public partial class About : ContentPage
{
    public About()
    {
        this.InitializeComponent();
        Task.Factory.StartNew(async () => await ((AboutViewModel)this.BindingContext).LoadTexts());
    }
}