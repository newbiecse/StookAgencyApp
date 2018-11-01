using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using StookAgencyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StookAgencyApp.Services
{
    public class ApplicationRoleManager : RoleManager<Role>
    {
        public ApplicationRoleManager(IRoleStore<Role> store) : base(store)
        {
        }

        public static RoleManager<Role> Create(IOwinContext context)
        {
            var roleStore = context.Get<IRoleStore<Role>>();
            return new ApplicationRoleManager(roleStore);
        }
    }
}