using MudBlazor;

namespace Learning.Web.Constants;

public static class DialogOptionConstant
{
    public static readonly DialogOptions MediumWidth = new DialogOptions()
    {
        NoHeader = true,
        CloseButton = true,
        BackdropClick = false,
        MaxWidth = MaxWidth.Medium
    };
    
    public static readonly DialogOptions SmallWidth = new DialogOptions()
    {
        NoHeader = true,
        CloseButton = true,
        BackdropClick = false,
        MaxWidth = MaxWidth.Small
    };

    public static readonly DialogOptions ExtraExtraLargeWidth = new DialogOptions()
    {
        NoHeader = true,
        CloseButton = true,
        BackdropClick = false,
        MaxWidth = MaxWidth.ExtraExtraLarge,
    };
}
