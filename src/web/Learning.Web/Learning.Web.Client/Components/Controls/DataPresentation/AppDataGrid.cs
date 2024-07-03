using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Web.Client.Components.Controls;
[CascadingTypeParameter("T")]
public class AppDataGrid<T> : MudDataGrid<T>
{
    public AppDataGrid()
    {
        Style = "height: 100%;";
        Height = "calc(100% - 64px)";
    }
}
