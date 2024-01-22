
using Microsoft.AspNetCore.Identity;

namespace Learning.Web.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<Learning.Domain.Identity.ApplicationUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<Learning.Domain.Identity.ApplicationUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
