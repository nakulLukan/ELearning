using Learning.Web.Client.Models.General;
using System.ComponentModel.DataAnnotations;

namespace Learning.Web.Client.DataAnotationValidators;

public class FileRequiredAttribute : ValidationAttribute
{
    public FileRequiredAttribute()
    {
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            ErrorMessage = $"This is required.";
        }
    }

    public override bool IsValid(object? value)
    {
        var temp = value is BrowserFile file && !string.IsNullOrEmpty(file.Name);
        return temp;
    }
}
