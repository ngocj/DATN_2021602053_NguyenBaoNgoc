using Microsoft.AspNetCore.Mvc;

namespace SP.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
