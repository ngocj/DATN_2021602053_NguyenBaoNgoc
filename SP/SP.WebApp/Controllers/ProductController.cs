using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.ProductDto;

namespace SP.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/product";
        private HttpClient _httpClient;
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<ActionResult>  Index()
        {
            // get all product
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ProductViewDto>>(ApiUrl);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }
    }
}
