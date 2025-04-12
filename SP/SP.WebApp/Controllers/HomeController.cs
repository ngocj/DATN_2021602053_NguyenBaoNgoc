using Microsoft.AspNetCore.Mvc;
using SP.WebApp.Models;
using System.Diagnostics;

namespace SP.WebApp.Controllers
{
    public class HomeController : Controller
    {      
        public IActionResult Index()
        {
            return View();
        }    
    }
}
