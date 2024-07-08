using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Web.Client.Components.Controls;
[CascadingTypeParameter("T")]
public class AppMudDataGrid<T> : MudDataGrid<T>
{
    public AppMudDataGrid()
    {
        Style = "height: 100%;";
        Height = "calc(100% - 64px)";
    }
}
