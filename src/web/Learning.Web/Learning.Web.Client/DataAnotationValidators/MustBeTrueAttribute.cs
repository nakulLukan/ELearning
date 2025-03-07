using System.ComponentModel.DataAnnotations;

namespace Learning.Web.Client.DataAnotationValidators;

public class MustBeTrueAttribute : ValidationAttribute
{
    private readonly string _errorMessage;
    public MustBeTrueAttribute(string errorMessage)
    {
        _errorMessage = errorMessage;
    }
    public override bool IsValid(object value)
    {
        return value is bool boolValue && boolValue;
    }

    public override string FormatErrorMessage(string name)
    {
        return _errorMessage;
    }
}
