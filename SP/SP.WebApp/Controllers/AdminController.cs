using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.FeedbackDto;
using SP.Application.Dto.ImageDto;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.ProductDto;
using SP.Application.Dto.ProductVariantDto;
using SP.Application.Dto.UserDto;

namespace SP.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api";
        private readonly HttpClient _httpClient;

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
        //get all products
        public async Task<ActionResult> GetAllProduct()
        {

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ProductViewDto>>($"{ApiUrl}/product");
            return View(response);
        }
        // get all brand
        public async Task<ActionResult> GetAllBrand()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<BrandViewDto>>($"{ApiUrl}/brand");
            return View(response);
        }
        // get all image
        public async Task<ActionResult> GetAllImage()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ImageFileDto>>($"{ApiUrl}/imageFile");
            return View(response);
        }
        // get all productVariant
        public async Task<ActionResult> GetAllProductVariant()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<VariantViewDto>>($"{ApiUrl}/productVariant");
            return View(response);
        }
        // get all order
        public async Task<ActionResult> GetAllOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrderViewDto>>($"{ApiUrl}/order");
            return View(response);
        }
        // feedback
        public async Task<ActionResult> GetAllFeedback()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<FeedbackViewDto>>($"{ApiUrl}/feedback");
            return View(response);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
