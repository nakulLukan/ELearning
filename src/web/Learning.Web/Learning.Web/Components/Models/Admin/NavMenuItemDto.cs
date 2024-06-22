namespace Learning.Web.Components.Models.Admin;

public class NavMenuItemDto
{
    public required string MenuName { get; set; }
    public string Icon { get; set; }
    public required string Route { get; set; }

    public required string[] AllowedRoles { get; set; }
}
