using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.UserDto;

namespace SP.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api";
        private HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<ActionResult> GetAllCategory()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryViewDto>>($"{ApiUrl}/category");
            return View(response);
        }
        public async Task<ActionResult> GetAllUser()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<UserViewDto>>($"{ApiUrl}/user");
            return View(response);
        }    
        public IActionResult Index()
        {
            return View();
        }
    }
}
