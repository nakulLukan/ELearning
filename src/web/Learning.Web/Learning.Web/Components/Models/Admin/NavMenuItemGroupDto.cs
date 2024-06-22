namespace Learning.Web.Components.Models.Admin;

public class NavMenuItemGroupDto
{
    public required string GroupName { get; set; }
    public string Icon { get; set; }

    public required List<NavMenuItemDto> MenuItems { get; set; }
}
