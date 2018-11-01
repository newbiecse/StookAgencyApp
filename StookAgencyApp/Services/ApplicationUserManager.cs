using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using StookAgencyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StookAgencyApp.Services
{
    public class ApplicationUserManager : UserManager<User>, IDisposable
    {
        public ApplicationUserManager(IUserStore<User> store) : base(store)
        {
            this.UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            this.UserTokenProvider = new DataProtectorTokenProvider<User>(new DpapiDataProtectionProvider("PriveleaseAuto").Create());
        }

        public static UserManager<User> Create(IdentityFactoryOptions<UserManager<User>> options, IOwinContext context)
        {
            ApplicationUserManager manager = new ApplicationUserManager(context.Get<IUserStore<User>>());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            return manager;
        }
    }
}