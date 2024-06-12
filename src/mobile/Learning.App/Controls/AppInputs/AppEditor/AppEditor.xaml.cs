using Learning.App.Controls.AppInputs;
using System.Runtime.CompilerServices;

namespace Learning.App.Controls.AppInputs;

public partial class AppEditor : AppControlBase
{
    public AppEditor()
    {
        InitializeComponent();
        LblCaption = caption;
    }

    #region Text
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AppEditor), defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion

    #region Placeholder
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(AppEditor),
            defaultBindingMode: BindingMode.OneTime);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    #endregion

    #region Title
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(AppEditor),
            defaultBindingMode: BindingMode.OneTime);

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
            typeof(AppEditor),
            defaultBindingMode: BindingMode.OneWay);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }
    #endregion

    #region BackgroundColor
    public static readonly new BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(AppEditor),
            defaultBindingMode: BindingMode.OneWay);

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }
    #endregion

    #region RightIcon
    public static readonly BindableProperty RightIconProperty =
        BindableProperty.Create(
            nameof(RightIcon),
            typeof(string),
            typeof(AppEditor),
            defaultBindingMode: BindingMode.OneWay);

    public string RightIcon
    {
        get => (string)GetValue(RightIconProperty);
        set => SetValue(RightIconProperty, value);
    }
    #endregion

    #region IsReadOnly
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(AppEditor),
            defaultValue: false,
            defaultBindingMode: BindingMode.OneWay);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
    #endregion

    #region Keyboard
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(
            nameof(Keyboard),
            typeof(Keyboard),
            typeof(AppEditor),
            defaultValue: Keyboard.Text,
            defaultBindingMode: BindingMode.OneTime);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }
    #endregion

    #region AutoSize
    public static readonly BindableProperty AutoSizeProperty = BindableProperty.Create(
        nameof(AutoSize),
        typeof(EditorAutoSizeOption),
        typeof(AppEditor),
        defaultValue: EditorAutoSizeOption.Disabled,
        BindingMode.OneTime);

    public EditorAutoSizeOption AutoSize
    {
        get { return (EditorAutoSizeOption)GetValue(AutoSizeProperty); }
        set { SetValue(AutoSizeProperty, value); }
    }
    #endregion

    #region MaxGrowHeight
    public static readonly BindableProperty MaxGrowHeightProperty = BindableProperty.Create(
        nameof(MaxGrowHeight),
        typeof(double),
        typeof(AppEditor),
        defaultValue: double.PositiveInfinity,
        BindingMode.OneTime);

    public double MaxGrowHeight
    {
        get { return (double)GetValue(MaxGrowHeightProperty); }
        set { SetValue(MaxGrowHeightProperty, value); }
    }
    #endregion

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == TextProperty.PropertyName)
        {
            appEditorBase.Text = Text;
        }
    }

    private void EntryBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Whenever the app base entry control visual state changes (mostly 'Focused'), then change the state of
        // this component as well.
        if (e.PropertyName == Editor.IsFocusedProperty.PropertyName)
        {
            if (appEditorBase.IsFocused)
            {
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Focused);

                // The reference to last text input is required in order to close the soft keyboard incase it is not closed when 
                // the focus changes. At present the keyboard is closed when app picker is selected.
                // App picker bottomsheet is displayed below keyboard when app picker is selected while app entry/editor is in focus.
                AppViewStateController.LastActiveTextInput = appEditorBase;
            }
            else
            {
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
            }
        }
        if (e.PropertyName == Editor.TextProperty.PropertyName)
        {
            if (_validatePropertyType != null && Validator != null)
            {
                _validatePropertyType.Invoke(Validator, new[] { ValidationField });
            }
        }
    }
}
