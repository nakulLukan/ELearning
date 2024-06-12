using AsyncAwaitBestPractices;
using Learning.Core.Exceptions;
using Learning.Core.Extensions;
using Learning.Core.Utilities;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Learning.App.Controls.AppInputs;

public class AppControlBase : ContentView
{
    protected Label LblCaption;

    #region State

    /// <summary>
    /// Different states of the entry
    /// </summary>
    public static readonly BindableProperty StateProperty =
        BindableProperty.Create(nameof(State),
                                typeof(AppInputState),
                                typeof(AppControlBase),
                                AppInputState.Default,
                                defaultBindingMode: BindingMode.OneWay);

    public AppInputState State
    {
        get => (AppInputState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }
    #endregion

    #region IsValid
    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(AppControlBase), true, defaultBindingMode: BindingMode.OneWay);

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }
    #endregion

    #region Validator
    public static readonly BindableProperty ValidatorProperty =
        BindableProperty.Create(
            nameof(Validator),
            typeof(object),
            typeof(AppControlBase),
            defaultBindingMode: BindingMode.OneWay);

    /// <summary>
    /// Instance of a <see cref="AppAbstractValidator{T}"/>
    /// </summary>
    public object Validator
    {
        get => (object)GetValue(ValidatorProperty);
        set => SetValue(ValidatorProperty, value);
    }

    protected MethodInfo _validatePropertyType = null;

    #endregion

    #region ValidationField

    /// <summary>
    /// Name of the property binding to this control.
    /// </summary>
    public static readonly BindableProperty ValidationFieldProperty = BindableProperty.Create(
        nameof(ValidationField),
        typeof(string),
        typeof(AppControlBase),
        defaultBindingMode: BindingMode.OneTime);

    public string ValidationField
    {
        get => (string)GetValue(ValidationFieldProperty);
        set => SetValue(ValidationFieldProperty, value);
    }
    #endregion

    #region ValidatorErrors
    /// <summary>
    /// Collection of errors associated to the property <see cref="ValidationField"/>
    /// </summary>
    public static readonly BindableProperty ValidatorErrorsProperty = BindableProperty.Create(
        nameof(ValidatorErrors),
        typeof(IDictionary<string, IEnumerable<string>>),
        typeof(AppControlBase),
        defaultBindingMode: BindingMode.OneWay);

    public IDictionary<string, IEnumerable<string>> ValidatorErrors
    {
        get => (IDictionary<string, IEnumerable<string>>)GetValue(ValidatorErrorsProperty);
        set => SetValue(ValidatorErrorsProperty, value);
    }
    #endregion

    #region CaptionColor
    public static readonly BindableProperty CaptionColorProperty =
        BindableProperty.Create(
            nameof(CaptionColor),
            typeof(Color),
            typeof(AppControlBase),
            defaultBindingMode: BindingMode.OneWay);

    public Color CaptionColor
    {
        get => (Color)GetValue(CaptionColorProperty);
        set => SetValue(CaptionColorProperty, value);
    }
    #endregion

    protected void TriggerValidation()
    {
        // Validate field
        if (_validatePropertyType != null)
        {
            Task.Run(async () =>
            {
                // This delay runs the validation after the value gets updated in the view model
                await Task.Delay(100);

                if (Validator == null)
                {
                    return;
                }
                _validatePropertyType.Invoke(Validator, new[] { ValidationField });
            }).SafeFireAndForget();
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == ValidatorErrorsProperty.PropertyName && ValidatorErrors != null)
        {
            if (string.IsNullOrEmpty(ValidationField))
            {
                throw new UIException(nameof(AppControlBase), $"'{nameof(ValidationField)}' should be set if validation errors has to be shown.");
            }
            // Set IsValid property to false if there are any errors
            IsValid = !ValidatorErrors.ContainsKey(ValidationField);
            if (ValidatorErrors.ContainsKey(ValidationField))
            {
                // Choose the first error from collection of errors
                LblCaption.Text = ValidatorErrors[ValidationField]
                    .First();
            }
            else
            {
                LblCaption.Text = string.Empty;
            }
        }
        else if (propertyName == IsValidProperty.PropertyName)
        {
            // Change the state of the control to error if there are validation errors.
            State = IsValid ? AppInputState.Default : AppInputState.Error;
        }
        else if (propertyName == ValidatorProperty.PropertyName)
        {
            if (Validator == null)
            {
                return;
            }
            // Get the instance of the app abstract validator object
            if (!Validator.IsInstanceOfGenericType(typeof(AppAbstractValidator<>)))
            {
                throw new UIException(nameof(AppControlBase), $"{ValidatorProperty.PropertyName} must be an instance of 'AppAbstractValidator<>'");
            }

            // This is later used to invoke the ValidateProperty()
            _validatePropertyType = Validator.GetType().GetMethod(nameof(AppAbstractValidator<object>.ValidateProperty));
        }
    }
}