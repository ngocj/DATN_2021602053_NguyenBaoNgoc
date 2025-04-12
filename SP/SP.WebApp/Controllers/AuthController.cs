using Microsoft.AspNetCore.Mvc;

namespace SP.WebApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
