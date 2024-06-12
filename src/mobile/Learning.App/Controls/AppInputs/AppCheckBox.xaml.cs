namespace Learning.App.Controls.AppInputs;

public partial class AppCheckBox : ContentView
{
    public AppCheckBox()
    {
        InitializeComponent();
    }

    #region Title
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(AppCheckBox),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.OneWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    #endregion

    #region BorderColor
    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(AppCheckBox),
            defaultBindingMode: BindingMode.OneWay);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }
    #endregion

    #region TextColor
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(AppCheckBox),
            defaultBindingMode: BindingMode.OneWay);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }
    #endregion

    #region CheckBoxBackgroundColor
    public static readonly BindableProperty CheckBoxBackgroundColorProperty =
        BindableProperty.Create(
            nameof(CheckBoxBackgroundColor),
            typeof(Color),
            typeof(AppCheckBox),
            defaultBindingMode: BindingMode.OneWay);

    public Color CheckBoxBackgroundColor
    {
        get => (Color)GetValue(CheckBoxBackgroundColorProperty);
        set => SetValue(CheckBoxBackgroundColorProperty, value);
    }
    #endregion

    #region FontSize
    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(AppCheckBox),
        defaultBindingMode: BindingMode.OneTime);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }
    #endregion

    #region IsChecked
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(AppCheckBox),
        false,
        defaultBindingMode: BindingMode.TwoWay);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    #endregion

    #region Size
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
        nameof(Size),
        typeof(double),
        typeof(AppCheckBox),
        defaultValue: 14D,
        defaultBindingMode: BindingMode.OneTime);

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
    #endregion

    #region CheckBoxSize
    public static readonly BindableProperty CheckBoxSizeProperty = BindableProperty.Create(
        nameof(CheckBoxSize),
        typeof(double),
        typeof(AppCheckBox),
        defaultBindingMode: BindingMode.OneTime);

    public double CheckBoxSize
    {
        get => (double)GetValue(CheckBoxSizeProperty);
        set => SetValue(CheckBoxSizeProperty, value);
    }
    #endregion

    private async void CheckBox_Clicked(object sender, EventArgs e)
    {
        VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Focused);
        IsChecked = !IsChecked;
        await Task.Delay(50);
        VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
    }
}