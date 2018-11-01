using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StookAgencyApp.Models;
using System;

namespace StookAgencyApp.Services
{
    public class ApplicationRoleStore : RoleStore<Role>, IRoleStore<Role>, IRoleStore<Role, string>, IDisposable
    {
        public ApplicationRoleStore(AppDbContext context) : base(context)
        {
        }
    }
}