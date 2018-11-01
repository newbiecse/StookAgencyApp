using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StookAgencyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StookAgencyApp.Services
{
    public class ApplicationUserStore : UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IUserLoginStore<User>, IUserClaimStore<User>, IUserPasswordStore<User>, IUserSecurityStampStore<User>, IUserStore<User>, IUserEmailStore<User>, IUserLockoutStore<User, string>, IDisposable
    {
        public ApplicationUserStore(AppDbContext context) : base(context)
        {
        }
    }
}