using MudBlazor;

namespace Learning.Web.Client.Components.Controls;

public class AppDatePicker : MudDatePicker
{
    public AppDatePicker()
    {
        Variant = Variant.Outlined;
        PickerVariant = PickerVariant.Dialog;
        Margin = Margin.Dense;
    }
}
