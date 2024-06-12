using CommunityToolkit.Mvvm.Input;
using Learning.App.Helpers;
using Learning.Core.Contracts.PageModels.Base;
using Learning.Core.Contracts.Services;
using Learning.Core.Helpers.Extensions;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Serilog;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Learning.App.Pages;

public partial class ContentPageBase : ContentPage, IQueryAttributable
{
    private readonly IPageService _pageService;
    private readonly ILogger _logger;

    #region IsBackButtonVisible
    public static readonly BindableProperty IsBackButtonVisibleProperty = BindableProperty.Create(
        nameof(IsBackButtonVisible),
        typeof(bool),
        typeof(ContentPageBase),
        true);

    public bool IsBackButtonVisible
    {
        get => (bool)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }
    #endregion

    #region ToolbarView
    public static readonly BindableProperty ToolbarViewProperty = BindableProperty.Create(
        nameof(ToolbarView),
        typeof(BindableObject),
        typeof(ContentPageBase));

    public BindableObject ToolbarView
    {
        get => (BindableObject)GetValue(ToolbarViewProperty);
        set => SetValue(ToolbarViewProperty, value);
    }
    #endregion

    #region BackCommand
    public static readonly BindableProperty BackCommandProperty = BindableProperty.Create(
        nameof(BackCommand),
        typeof(ICommand),
        typeof(ContentPageBase));

    public ICommand BackCommand
    {
        get => (ICommand)GetValue(BackCommandProperty);
        set => SetValue(BackCommandProperty, value);
    }
    #endregion

    #region IsTabbarVisible
    public static readonly BindableProperty IsTabbarVisibleProperty = BindableProperty.Create(
        nameof(IsTabbarVisible),
        typeof(bool),
        typeof(ContentPageBase),
        false);

    public bool IsTabbarVisible
    {
        get => (bool)GetValue(IsTabbarVisibleProperty);
        set => SetValue(IsTabbarVisibleProperty, value);
    }
    #endregion

    protected ContentPageBase()
    {
        var titleViewTemplate = ResourceHelper.FindResource<ControlTemplate>("NavTitleViewTemplate");
        var titleView = new ContentView()
        {
            ControlTemplate = titleViewTemplate
        };
        titleView.BindingContext = this;
        Shell.SetTitleView(this, titleView);

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior() { IsVisible = false });
        Shell.SetTabBarIsVisible(this, false);
        _logger = Log.ForContext(GetType());

        On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetUseSafeArea(false);

        _pageService = AppServiceProvider.ServiceProvider.GetRequiredService<IPageService>();
        PropertyChanged += OnPropertyChanged;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (ToolbarView != null)
            ToolbarView.BindingContext = BindingContext;

        if (BindingContext is INotifyPropertyChanged bindingContext)
        {
            bindingContext.PropertyChanged += OnPropertyChanged;
        }
    }

    protected override async void OnAppearing()
    {
        if (BindingContext is not IPageModelBase pageModel) return;

        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            await pageModel.InitializeAsyncCommand.ExecuteAsync(null);
            stopwatch.Stop();
            _logger.Verbose("Did Initialize {PageModel} in {ElapsedMilliseconds} milliseconds", pageModel.GetType().ToString(), stopwatch.ElapsedMilliseconds);
        }
        catch (Exception e)
        {
            stopwatch.Stop();
            _logger.Error(e, "Initialize {PageModel} failed in {ElapsedMilliseconds} milliseconds", pageModel.GetType().ToString(), stopwatch.ElapsedMilliseconds);
        }
    }

    protected override async void OnDisappearing()
    {
        PropertyChanged -= OnPropertyChanged;

        if (BindingContext is not IPageModelBase pageModel) return;
        try
        {
            await pageModel.StopAsync();
        }
        catch (Exception e)
        {
            _logger.Error(e, "StopAsync failed");
        }

    }

    [RelayCommand]
    public virtual async Task NavigateBackAsync()
    {
        var cancelEventArgs = new CancelEventArgs();
        if (BackCommand != null && BackCommand.CanExecute(null))
        {
            BackCommand.Execute(cancelEventArgs);
        }
        else if (IsBackButtonVisible)
        {
            // Popup page only if back button is visible
            await _pageService.PopAsync();
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == IsTabbarVisibleProperty.PropertyName && IsTabbarVisible)
        {
            var titleViewTemplate = ResourceHelper.FindResource<ControlTemplate>("ShellNavTitleViewTemplate");
            ((ContentView)Shell.GetTitleView(this)).ControlTemplate = titleViewTemplate;
            Shell.SetTabBarIsVisible(this, IsTabbarVisible);
        }
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == null) return;

        if (args.PropertyName.Equals("IsBusy"))
        {
            if (sender is IPageModelBase pageModelBase)
            {
                if (pageModelBase.IsBusy)
                    _pageService.DisplayProgressHUD();
                else
                    _pageService.DismissProgressHUD();
            }
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (NavigateBackCommand != null && NavigateBackCommand.CanExecute(null))
        {
            NavigateBackCommand.Execute(null);
        }
        return true;
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is not IPageModelBase pageModel) return;
        try
        {
            pageModel.ApplyQueryAttributes(query);
        }
        catch (Exception e)
        {
            _logger.Error(e, "ApplyQueryAttributes failed for {PageModel} with attributes {@Query}", pageModel.GetType().ToString(), query);
        }
    }
}