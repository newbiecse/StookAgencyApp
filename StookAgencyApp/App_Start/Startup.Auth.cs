using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using StookAgencyApp;
using StookAgencyApp.Extensions;
using StookAgencyApp.Models;
using StookAgencyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Startup))]
namespace StookAgencyApp
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<IUnitOfWork>());
            app.CreatePerOwinContext<IUserStore<User>>(() => DependencyResolver.Current.GetService<IUserStore<User>>());
            app.CreatePerOwinContext<IRoleStore<Role>>(() => DependencyResolver.Current.GetService<IRoleStore<Role>>());
            app.CreatePerOwinContext<UserManager<User>>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<SignInManager<User, string>>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                       validateInterval: TimeSpan.FromMinutes(30),
                       regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //HttpConfiguration config = new HttpConfiguration();
            //WebApiConfig.Register(config);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}