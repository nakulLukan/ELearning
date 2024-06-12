using Learning.Core.PageModels.Startup;

namespace Learning.App.Pages.Startup;

public partial class PlaygroundPage : ContentPageBase
{
    public PlaygroundPage(PlaygroundPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}