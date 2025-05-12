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
        public async Task<ActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productCreateDto);
            }
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, productCreateDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something went wrong");
            return View(productCreateDto);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ProductViewDto productViewDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewDto);
            }
            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/{productViewDto.Id}", productViewDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something went wrong");
            return View(productViewDto);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

    }
}
