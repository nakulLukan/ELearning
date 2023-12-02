using IdentityServerHost.Quickstart.UI;
using Learning.Persistent.Identity.Data.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Learning.Persistent.Identity.Quickstart.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRegistrationController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new AppUser { UserName = model.Username, Email = model.Username };
            var result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Index");
            }
        }
    }
}
