using MudBlazor;

namespace Learning.Web.Client.Constants;

public static class DialogOptionConstant
{
    public static readonly DialogOptions MediumWidth = new DialogOptions()
    {
        NoHeader = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Medium
    };
    
    public static readonly DialogOptions SmallWidth = new DialogOptions()
    {
        NoHeader = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small
    };
}
