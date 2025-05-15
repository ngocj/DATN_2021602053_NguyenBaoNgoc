using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.DiscountDto;
using SP.Application.Dto.ProductDto;
using SP.Application.Dto.ProductVariantDto;

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
        public async Task<ActionResult> Create([FromForm] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productCreateDto);
            }

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, productCreateDto);

            if (response.IsSuccessStatusCode)
            {
                // Đọc phản hồi trả về object có Id
                var content = await response.Content.ReadFromJsonAsync<ProductViewDto>();

                if (content?.Id != null)
                {
                    return RedirectToAction("CreateProductVariant", "ProductVariant", new { ProductId = content.Id });
                }

                ModelState.AddModelError("", "Không thể lấy ProductId từ phản hồi.");
                return View(productCreateDto);
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi tạo sản phẩm.");
            return View(productCreateDto);
        }
    
        public async Task<ActionResult> Edit(int id)
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
            var response = await _httpClient.GetFromJsonAsync<ProductUpdateDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }

            // FIX: Truyền selected value
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName", response.BrandId);
            ViewBag.Categories = new SelectList(subCategories, "Id", "Name", response.SubCategoryId);
            ViewBag.Discounts = discounts.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Percent}%",
                Selected = (d.Id == response.DiscountId) // Discount dùng SelectListItem nên selected xử lý riêng
            }).ToList();

            return View(response);

        }
        [HttpPost]
        public async Task<ActionResult> Edit(ProductUpdateDto productUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(productUpdate);
            }
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, productUpdate);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllProduct", "Admin");
            }
            ModelState.AddModelError("", "Something went wrong");
            return View(productUpdate);
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
