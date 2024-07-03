using Learning.Shared.Common.Extensions;
using Learning.Web.Client.Models.General;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Learning.Web.Client.DataAnotationValidators;

public class MaxFileSizeAttribute : ValidationAttribute
{
    long _maxSize;
    public MaxFileSizeAttribute(long maxSize = 1_000)
    {
        _maxSize = maxSize;
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            ErrorMessage = $"File size cannot be greater than {_maxSize.ToFileSizeString()}";
        }
    }

    public override bool IsValid(object? value)
    {
        if (value is IBrowserFile file)
        {
            return file.Size > _maxSize ? false : true;
        }
        else if (value is BrowserFile browserFile) 
        {
            return browserFile.Size > _maxSize ? false : true;
        }

        return true;
    }
}
