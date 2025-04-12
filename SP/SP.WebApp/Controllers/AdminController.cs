using Microsoft.AspNetCore.Mvc;

namespace SP.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
