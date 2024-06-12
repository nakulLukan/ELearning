using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Learning.Core.Utilities;

/// <summary>
/// Validator class to validate models binding to pages
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppAbstractValidator<T> : AbstractValidator<T>, INotifyPropertyChanged
{
    private bool _isSubmittedOnce = false;
    private readonly bool _shouldValidateOnlyOnSubmit = true;
    private readonly T _validationObject;
    public event PropertyChangedEventHandler? PropertyChanged;

    #region Errors
    /// <summary>
    /// Collection of errors received after executing <see cref="Validate(ValidationContext{T})"/>
    /// Key: Property Name
    /// Value: List of errors
    /// </summary>
    public IDictionary<string, IEnumerable<string>>? Errors
    {
        get => _errors;
        set
        {
            _errors = value;
            OnPropertyChanged();
        }
    }
    private IDictionary<string, IEnumerable<string>>? _errors;
    #endregion Errors

    #region IsValid
    /// <summary>
    /// Flag to indicate whether this model is valid or not.
    /// </summary>
    public bool IsValid
    {
        get => _isValid;
        set
        {
            _isValid = value;
            OnPropertyChanged();
        }
    }
    private bool _isValid = true;
    #endregion IsValid

    /// <summary>
    /// Validator class to validate models binding to pages
    /// </summary>
    /// <param name="validationObject"></param>
    /// <param name="validationObject"></param>
    public AppAbstractValidator(T validationObject, bool shouldValidateOnlyOnSubmit = true)
    {
        _validationObject = validationObject;
        _shouldValidateOnlyOnSubmit = shouldValidateOnlyOnSubmit;
    }

    public bool ValidateProperty(string propertyName)
    {
        // Check if there exists any error for the given property
        // _isSubmittedOnce is used indicate that the form has been submitted atleast once.
        if ((_shouldValidateOnlyOnSubmit && !_isSubmittedOnce) || Validate().Errors.Any(x => x.PropertyName == propertyName))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates the given model
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Validation result</returns>
    public ValidationResult Validate()
    {
        _isSubmittedOnce = true;
        var validationResult = base.Validate(_validationObject);
        IsValid = validationResult.IsValid;
        Errors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .Where(x => x.Any())
            .ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage));
        return validationResult;
    }

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public void ClearValidation()
    {
        _isSubmittedOnce = false;
        IsValid = true;
        Errors = new Dictionary<string, IEnumerable<string>>();
    }
}
