using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json; // 👈 cần thiết để dùng ReadFromJsonAsync
using System.Threading.Tasks;
using static SP.Infrastructure.Repositories.Implement.OrderDetailRepository;

namespace SP.WebApp.Controllers
{
    public class OrderDetailController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/orderdetail";
        private readonly HttpClient _httpClient;

        public OrderDetailController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> TopSelling(int top = 5)
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/products/top-selling?top={top}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var topList = await response.Content.ReadFromJsonAsync<List<TopSellingVariant>>();
            return Json(topList);
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> TotalPending()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/products/total-pending");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<int>();
            return Json(total);
        }
        public async Task<IActionResult> TotalDelivered()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/products/total-delivered");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<int>();
            return Json(total);
        }

        public async Task<IActionResult> TotalCanceled()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/products/total-canceled");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<int>();
            return Json(total);
        }

        public async Task<IActionResult> TotalShipping()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/products/total-shipping");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<int>();
            return Json(total);
        }

        public async Task<IActionResult> TotalRevenue()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/revenue/total");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<decimal>();
            return Json(total);
        }

        public async Task<IActionResult> TotalRevenueByRange(DateTime from, DateTime to)
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/revenue/total-by-range?from={from:O}&to={to:O}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<decimal>();
            return Json(total);
        }
    }
}
