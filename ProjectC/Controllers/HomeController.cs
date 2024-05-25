using Microsoft.AspNetCore.Mvc;
using ProjectC.Data.Models;
using System.Diagnostics;

namespace ProjectC.Controllers
{
    [Route("{action}")]
    public class HomeController : Controller
    {

        public HomeController()
        {
            
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
