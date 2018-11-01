using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using StookAgencyApp.Models;

namespace StookAgencyApp.Services
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        private RoleManager<Role> _roleManager;

        public ApplicationSignInManager(UserManager<User> userManager, RoleManager<Role> roleManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            this._roleManager = roleManager;
        }


        public static SignInManager<User, string> Create(IdentityFactoryOptions<SignInManager<User, string>> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<UserManager<User>>(),
                context.Get<RoleManager<Role>>(),
                context.Authentication);
        }
    }
}