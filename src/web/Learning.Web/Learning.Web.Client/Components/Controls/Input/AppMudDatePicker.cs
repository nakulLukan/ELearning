using MudBlazor;

namespace Learning.Web.Client.Components.Controls;

public class AppMudDatePicker : MudDatePicker
{
    public AppMudDatePicker()
    {
        Variant = Variant.Outlined;
        PickerVariant = PickerVariant.Dialog;
        Margin = Margin.Dense;
    }
}
