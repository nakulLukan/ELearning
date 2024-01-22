using Learning.Domain.Identity;
using Learning.Infrasture.Persistence;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Learning.Infrastructure.Data.Seeder;

public class AdminUserSeeder
{
    readonly UserManager<ApplicationUser> _userManager;
    readonly ApplicationDbContext _dbContext;

    public AdminUserSeeder(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task Seed(List<DefaultUserDto> defaultUsers)
    {
        var existingUsernames = await _dbContext.Users
            .Where(x => defaultUsers.Select(x => x.Username.ToNormalizedString()).Contains(x.NormalizedUserName))
            .Select(x => x.UserName)
            .ToListAsync();

        foreach (var defaultUser in defaultUsers)
        {
            if (!existingUsernames.Contains(defaultUser.Username))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = defaultUser.Username,
                    Email = defaultUser.Username,
                    IsAdmin = true,
                };
                await _userManager.CreateAsync(adminUser, defaultUser.Password);
                await _userManager.AddToRoleAsync(adminUser, defaultUser.Role);
            }
        }
    }
}
