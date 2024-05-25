using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectC.Data.Models;
using System.Security.Claims;

namespace ProjectC.Services
{
    public abstract class CookieProvider
    {
        public static ClaimsPrincipal CreateCookie(User user)
        {
            Claim[] claims = [
                new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("ID", user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role.RoleName)
            ];
            
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
        
    }
}
