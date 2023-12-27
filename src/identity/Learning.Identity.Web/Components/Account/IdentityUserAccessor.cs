using Learning.Identity.Web.Data;
using Learning.Identity.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Learning.Identity.Web.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<Data.Entities.ApplicationUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<Data.Entities.ApplicationUser> GetRequiredUserAsync(HttpContext context)
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
