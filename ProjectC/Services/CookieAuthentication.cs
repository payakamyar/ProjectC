using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ProjectC.Contracts;
using ProjectC.Data.Context;
using ProjectC.Data.DTO;
using ProjectC.Data.Models;
using ProjectC.Utils;
using System.Security.Claims;
using System.Text;

namespace ProjectC.Services
{
    public class CookieAuthentication : IAuthentication
    {
        private readonly ProjectContext _projectContext;
        private readonly IHttpContextAccessor _httpContext;
        public CookieAuthentication(ProjectContext projectContext, IHttpContextAccessor httpContext)
        {
            _projectContext = projectContext;
            _httpContext = httpContext;

        }
        public AuthenticationResult Authenticate(UserLoginDTO user)
        {
            User? foundUser = _projectContext.Users.Include(p => p.Role).Where(u => u.UserName == user.UserName &&
			u.Password == PasswordUtility.GetPasswordHash(user.Password)).FirstOrDefault();
            
            if(foundUser is null)
                return new AuthenticationResult{ WasSuccessful = false, ErrorMessage = "Username or Password is incorrect."};

            ClaimsPrincipal claimsPrincipal = CookieProvider.CreateCookie(foundUser);
            _httpContext.HttpContext.SignInAsync(claimsPrincipal);
            return new AuthenticationResult();
        }

        


    }
}
