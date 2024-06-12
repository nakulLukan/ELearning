using Learning.Core.PageModels.Startup;

namespace Learning.App.Pages.Startup;

public partial class SplashPage : ContentPage
{
    public SplashPage(SplashPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}