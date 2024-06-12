using System.Windows.Input;

namespace Learning.App.Controls.AppInputs;

public partial class AppToggleButton : ContentView
{
    public AppToggleButton()
    {
        InitializeComponent();
    }

    #region ToggleOffBgColor
    public static readonly BindableProperty ToggleOffBgColorProperty =
        BindableProperty.Create(
            nameof(ToggleOffBgColor),
            typeof(Color),
            typeof(AppToggleButton),
            defaultBindingMode: BindingMode.OneWay);

    public Color ToggleOffBgColor
    {
        get => (Color)GetValue(ToggleOffBgColorProperty);
        set => SetValue(ToggleOffBgColorProperty, value);
    }
    #endregion

    #region ToggleOnBgColor
    public static readonly BindableProperty ToggleOnBgColorProperty =
        BindableProperty.Create(
            nameof(ToggleOnBgColor),
            typeof(Color),
            typeof(AppToggleButton),
            defaultBindingMode: BindingMode.OneWay);

    public Color ToggleOnBgColor
    {
        get => (Color)GetValue(ToggleOnBgColorProperty);
        set => SetValue(ToggleOnBgColorProperty, value);
    }
    #endregion

    #region HandleColor
    public static readonly BindableProperty HandleColorProperty =
        BindableProperty.Create(
            nameof(HandleColor),
            typeof(Color),
            typeof(AppToggleButton),
            defaultBindingMode: BindingMode.OneWay);

    public Color HandleColor
    {
        get => (Color)GetValue(HandleColorProperty);
        set => SetValue(HandleColorProperty, value);
    }
    #endregion

    #region Value
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(bool),
            typeof(AppToggleButton),
            defaultBindingMode: BindingMode.TwoWay);

    public bool Value
    {
        get => (bool)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    #endregion

    #region Command
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(AppToggleButton), null);

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }
    #endregion

    #region HeightValue
    public static readonly BindableProperty HeightValueProperty = BindableProperty.Create(
        nameof(HeightValue),
        typeof(double),
        typeof(AppToggleButton),
        defaultValue: 20D,
        BindingMode.OneTime);

    public double HeightValue
    {
        get { return (double)GetValue(HeightValueProperty); }
        set { SetValue(HeightValueProperty, value); }
    }
    #endregion

    #region WidthValue
    public static readonly BindableProperty WidthValueProperty = BindableProperty.Create(
        nameof(WidthValue),
        typeof(double),
        typeof(AppToggleButton),
        defaultValue: 40D,
        BindingMode.OneTime);

    public double WidthValue
    {
        get { return (double)GetValue(WidthValueProperty); }
        set { SetValue(WidthValueProperty, value); }
    }
    #endregion

    #region HandleSize
    public static readonly BindableProperty HandleSizeProperty = BindableProperty.Create(
        nameof(HandleSize),
        typeof(double),
        typeof(AppToggleButton),
        defaultValue: 16D,
        BindingMode.OneTime);

    public double HandleSize
    {
        get { return (double)GetValue(HandleSizeProperty); }
        set { SetValue(HandleSizeProperty, value); }
    }
    #endregion

    private void Button_Pressed(object sender, EventArgs e)
    {
        Value = !Value;
        if (Command != null && Command.CanExecute(null))
        {
            Command.Execute(null);
        }
    }
}