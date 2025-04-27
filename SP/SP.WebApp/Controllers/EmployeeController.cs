using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.EmployeeDto;

namespace SP.WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/employee";
        private HttpClient _httpClient;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult CreateEmployee()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateEmployee(EmployeeCreateDto employeeCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, employeeCreateDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllEmployee", "Admin");
            }
            return View(employeeCreateDto);     
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
