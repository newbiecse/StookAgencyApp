using Microsoft.AspNet.Identity;
using StookAgencyApp.Models;
using StookAgencyApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StookAgencyApp.Extensions
{
    public static class UserExtensions
    {
        //public static UserInformation GetInformation(this User user)
        //{
        //    if (!string.IsNullOrEmpty(user.CustomAttributes))
        //    {
        //        try
        //        {
        //            return XmlHelper.XmlToModel<UserInformation>(user.CustomAttributes);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    return null;
        //}

        //public static void SetInformation(this User user, UserInformation info)
        //{
        //    user.CustomAttributes = XmlHelper.ModelToXml(info);
        //}

        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this User user, ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            IList<Claim> claims = await manager.GetClaimsAsync(user.Id);

            Claim accessToken = claims.FirstOrDefault(c => c.Type == "AccessToken");
            if (accessToken != null) await manager.RemoveClaimAsync(user.Id, accessToken);
            await manager.AddClaimAsync(user.Id, userIdentity.FindFirst("AccessToken"));

            Claim refreshToken = claims.FirstOrDefault(c => c.Type == "RefreshToken");
            if (refreshToken != null) await manager.RemoveClaimAsync(user.Id, refreshToken);
            await manager.AddClaimAsync(user.Id, userIdentity.FindFirst("RefreshToken"));

            Claim tokenExpires = claims.FirstOrDefault(c => c.Type == "TokenExpires");
            if (tokenExpires != null) await manager.RemoveClaimAsync(user.Id, tokenExpires);
            await manager.AddClaimAsync(user.Id, userIdentity.FindFirst("TokenExpires"));

            return userIdentity;
        }
    }
}