using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.CategoryDto;

namespace SP.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/category";
        private HttpClient _httpClient;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult CreateCategory()
        {
           return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, categoryCreateDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllCategory", "Admin");
            }
            return View(categoryCreateDto);     
        }

        public async Task<ActionResult> UpdateCategory(int id)
        {
            var categoryViewDto = await _httpClient.GetFromJsonAsync<CategoryViewDto>($"{ApiUrl}/{id}");
            return View(categoryViewDto);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCategory(CategoryViewDto categoryUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, categoryUpdateDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllCategory", "Admin");
            }
            return View(categoryUpdateDto);
        }
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllCategory", "Admin");
            }
            return View();
        }
    }
}
