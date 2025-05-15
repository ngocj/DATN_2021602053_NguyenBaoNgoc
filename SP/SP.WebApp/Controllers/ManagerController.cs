using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.DiscountDto;
using SP.Application.Dto.EmployeeDto;
using SP.Application.Dto.OrderDto;

namespace SP.WebApp.Controllers
{

    public class ManagerController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api";
        private HttpClient _httpClient;

        public ManagerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        // get all discount
        public async Task<ActionResult> GetAllDiscount()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<DiscountViewDto>>($"{ApiUrl}/discount");
            return View(response);
        }
        // get all employee
        public async Task<ActionResult> GetAllEmployee()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeViewDto>>($"{ApiUrl}/employee");
            return View(response);
        }
        public IActionResult Index()
        {
            return View();
        }

        // get all order
        public async Task<ActionResult> GetAllOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrderViewDto>>($"{ApiUrl}/order");
            return View(response);
        }

    }
}
