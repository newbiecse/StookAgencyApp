using Microsoft.AspNet.Identity;
using StookAgencyApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StookAgencyApp
{
    public class AppDbInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            try
            {
                //var roleManager = DependencyResolver.Current.GetService<RoleManager<Role>>();
                var userManager = DependencyResolver.Current.GetService<UserManager<User>>();

                var user = userManager.FindByName("admin");
                if (user == null)
                {
                    user = new User
                    {
                        UserName = "admin@stockagency.com",
                        Email = "admin@stockagency.com"
                    };

                    userManager.Create(user, "p@ssword");
                }

                if (!context.Customers.Any())
                {
                    context.Customers.Add(new Customer
                    {
                        Name = "Lionel Mcgee",
                        DateJoined = new DateTime(2016, 12, 20),
                        Email = "interdum.sed@vehicula.ca"
                    });

                    context.Customers.Add(new Customer
                    {
                        Name = "Harding Peterson",
                        DateJoined = new DateTime(2016, 11, 16),
                        Email = "viae.posuere@sodales.ca"
                    });

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}