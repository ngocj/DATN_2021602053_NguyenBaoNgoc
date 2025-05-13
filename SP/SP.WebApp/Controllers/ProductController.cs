using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.DiscountDto;
using SP.Application.Dto.ProductDto;

namespace SP.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/product";
        private const string ApiUrl1 = "https://localhost:7131/api/";
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
        public async Task<IActionResult> Create()
        {
            // get all brands
            var brands = await _httpClient.GetFromJsonAsync<IEnumerable<BrandViewDto>>($"{ApiUrl1}brand");
            if (brands == null || !brands.Any())
            {
                ModelState.AddModelError(string.Empty, "No brands found.");
                return View();
            }

            // get all subcategories
            var subCategories = await _httpClient.GetFromJsonAsync<IEnumerable<SubCategoryViewDto>>($"{ApiUrl1}subcategory");
            if (subCategories == null || !subCategories.Any())
            {
                ModelState.AddModelError(string.Empty, "No subcategories found.");
                return View();
            }

            // get all discounts
            var discounts = await _httpClient.GetFromJsonAsync<IEnumerable<DiscountViewDto>>($"{ApiUrl1}Dicount");
            if (discounts == null || !discounts.Any())
            {
                ModelState.AddModelError(string.Empty, "No discounts found.");
                return View();
            }

            // Passing the data to the View
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName");
            ViewBag.Categories = new SelectList(subCategories, "Id", "Name");
            ViewBag.Discounts = discounts.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Percent}%"  // Thêm ký tự phần trăm
            }).ToList();


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
                return RedirectToAction("GetAllProduct", "Admin");
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
            // Kiểm tra xem sản phẩm có tồn tại không
            var product = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            if (product == null)
            {
                return NotFound();
            }

            // Gửi yêu cầu DELETE đến API
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // Xử lý lỗi nếu cần, ví dụ: return View("Error");
                return BadRequest("Không thể xóa sản phẩm.");
            }

            return RedirectToAction("GetAllProduct", "Admin");
        }


    }
}
