using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Contracts;
using ProjectC.Data.DTO;
using ProjectC.Data.Models;
using ProjectC.Services;
using System.Security.Claims;

namespace ProjectC.Controllers
{
    [Route("{controller}/{action}")]
    public class AccountController : Controller
    {
        private readonly IAuthentication _authentication;

        public AccountController(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost([FromForm] UserLoginDTO user)
        {
            AuthenticationResult result = _authentication.Authenticate(user);
            if(result.WasSuccessful)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.Error = result.ErrorMessage;
            return View("Login");
        }
        public IActionResult SignOut()
        {
            if(User.Identity?.IsAuthenticated == true)
            {
                HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }


	}
}
