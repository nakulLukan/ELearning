using CommunityToolkit.Maui.Behaviors;
using Learning.App.Constants;
using System.Runtime.CompilerServices;

namespace Learning.App.Controls.AppInputs;

public partial class AppEntry : AppControlBase
{
    public AppEntry()
    {
        InitializeComponent();
        LblCaption = caption;
    }

    #region Text
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AppEntry), defaultBindingMode: BindingMode.TwoWay);

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
            typeof(AppEntry),
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
            typeof(AppEntry),
            defaultBindingMode: BindingMode.OneTime);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    #endregion

    #region Mask
    public static readonly BindableProperty MaskProperty = BindableProperty.Create(nameof(Mask), typeof(string), typeof(AppEntry), defaultBindingMode: BindingMode.OneTime);

    public string Mask
    {
        get => (string)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }
    #endregion

    #region BorderColor
    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(AppEntry),
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
            typeof(AppEntry),
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
            typeof(AppEntry),
            defaultBindingMode: BindingMode.OneWay);

    public string RightIcon
    {
        get => (string)GetValue(RightIconProperty);
        set => SetValue(RightIconProperty, value);
    }
    #endregion

    #region LeftIcon
    public static readonly BindableProperty LeftIconProperty = BindableProperty.Create(nameof(LeftIcon), typeof(string), typeof(AppEntry), defaultBindingMode: BindingMode.OneTime);

    public string LeftIcon
    {
        get => (string)GetValue(LeftIconProperty);
        set => SetValue(LeftIconProperty, value);
    }
    #endregion

    #region IsReadOnly
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(AppEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.OneWay);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
    #endregion

    #region IsPassword
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(AppEntry),
            defaultBindingMode: BindingMode.OneWay);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
    #endregion

    #region Keyboard
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(
            nameof(Keyboard),
            typeof(Keyboard),
            typeof(AppEntry),
            defaultValue: Keyboard.Text,
            defaultBindingMode: BindingMode.OneTime);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }
    #endregion

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == TextProperty.PropertyName)
        {
            appEntryBase.Text = Text;
        }
        else if (propertyName == MaskProperty.PropertyName)
        {
            // Add masking if provided
            appEntryBase.Behaviors.Add(new MaskedBehavior()
            {
                Mask = Mask,
                UnmaskedCharacter = 'X'
            });
        }
    }

    /// <summary>
    /// Switches the password icon
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PasswordEyeIcon_Tapped(object sender, EventArgs e)
    {
        appEntryBase.IsPassword = !appEntryBase.IsPassword;

        // Switch password icon 
        passwordIcon.Glyph = !appEntryBase.IsPassword ? FaGlyph.EyeSlash : FaGlyph.Eye;
    }

    private void EntryBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Whenever the app base entry control visual state changes (mostly 'Focused'), then change the state of
        // this component as well.
        if (e.PropertyName == Entry.IsFocusedProperty.PropertyName)
        {
            if (appEntryBase.IsFocused)
            {
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Focused);

                // The reference to last text input is required in order to close the soft keyboard incase it is not closed when 
                // the focus changes. At present the keyboard is closed when app picker is selected.
                // App picker bottomsheet is displayed below keyboard when app picker is selected while app entry/editor is in focus.
                AppViewStateController.LastActiveTextInput = appEntryBase;
            }
            else
            {
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
            }
        }
        else if (e.PropertyName == Entry.TextProperty.PropertyName)
        {
            // This is to always place the cursor at the end of the input text.
            if (!string.IsNullOrEmpty(Mask))
            {
                ((Entry)sender).CursorPosition = Text.Length;
            }
            if (_validatePropertyType != null && Validator != null)
            {
                _validatePropertyType.Invoke(Validator, new[] { ValidationField });
            }
        }
    }
}
