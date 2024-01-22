using Learning.Infrasture.Persistence;
using Learning.Shared.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Learning.Infrastructure.Data.Seeder;

public class RoleSeeder
{
    readonly RoleManager<IdentityRole> _roleManager;
    readonly ApplicationDbContext _dbContext;

    public RoleSeeder(RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
    {
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task Seed()
    {
        string[] roles = ["super-admin", "user"];
        var existingRoles = await _dbContext.Roles
            .Where(x => roles.Select(x => x.ToNormalizedString()).Contains(x.NormalizedName))
            .Select(x => x.NormalizedName)
            .ToListAsync();

        foreach (var role in roles)
        {
            if (!existingRoles.Contains(role))
            {
                var newRole = new IdentityRole
                {
                    Name = role
                };
                await _roleManager.CreateAsync(newRole);
            }
        }
    }
}
