using MudBlazor;

namespace Learning.Web.Client.Components.Controls;

public class AppTextField<T> : MudTextField<T>
{
    public AppTextField()
    {
        Variant = Variant.Outlined;
        Margin = Margin.Dense;
    }
}
