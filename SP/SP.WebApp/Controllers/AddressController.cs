using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.ProvinceDto;

namespace SP.WebApp.Controllers
{
    public class AddressController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/address";
        private HttpClient _httpClient;

        public AddressController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> GetAllProvinces()
        {
            var provinces = await _httpClient.GetFromJsonAsync<IEnumerable<ProvinceViewDto>>(ApiUrl);
            return View(provinces);
        }
        public async Task<IActionResult> GetProvinceById(int id)
        {
            var province = await _httpClient.GetFromJsonAsync<ProvinceViewDto>($"{ApiUrl}/{id}");
            if (province == null)
            {
                return NotFound();
            }
            return View(province);
        }
    }
}
