using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StookAgencyApp.Models;
using StookAgencyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StookAgencyApp.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User, string> _signInManager;
        private UserManager<User> _userManager;

        public AccountController(SignInManager<User, string> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            SignInStatus result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                default:
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}