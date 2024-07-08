using MudBlazor;

namespace Learning.Web.Client.Components.Controls;

public class AppMudTextField<T> : MudTextField<T>
{
    public AppMudTextField()
    {
        Variant = Variant.Outlined;
        Margin = Margin.Dense;
    }
}
