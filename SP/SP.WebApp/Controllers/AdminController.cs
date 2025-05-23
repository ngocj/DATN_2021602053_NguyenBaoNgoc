using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.DiscountDto;
using SP.Application.Dto.FeedbackDto;
using SP.Application.Dto.ImageDto;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.ProductDto;
using SP.Application.Dto.ProductVariantDto;
using SP.Application.Dto.UserDto;
using SP.Domain.Entity;

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
        public async Task<ActionResult> GetAllProduct(int? brandId, int? SubcategoryId, bool? isActive)
        {
            var brands = await _httpClient.GetFromJsonAsync<IEnumerable<BrandViewDto>>($"{ApiUrl}/brand");
            if (brands == null || !brands.Any())
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy thương hiệu nào.");
                return View();
            }

            // get all subcategories
            var subCategories = await _httpClient.GetFromJsonAsync<IEnumerable<SubCategoryViewDto>>($"{ApiUrl}/subcategory");
            if (subCategories == null || !subCategories.Any())
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy danh mục nào");
                return View();
            }
            // get all products
            // Passing the data to the View
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName", brandId);
            ViewBag.Categories = new SelectList(subCategories, "Id", "Name", SubcategoryId);
            ViewBag.IsActive = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Tất cả", Value = null },
                new SelectListItem { Text = "Đang hoạt động", Value = "true" },
                new SelectListItem { Text = "Ngừng hoạt động", Value = "false" }
            }, "Value", "Text", isActive);

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ProductViewDto>>
            ($"{ApiUrl}/product/filter?SubcategoryId={SubcategoryId}&brandId={brandId}&isActive={isActive}");

            ViewBag.SelectedBrandId = brandId;
            ViewBag.SelectedSubCategoryId = SubcategoryId;
            ViewBag.SelectedIsActive = isActive;
            return View(response);
                    
        }
        public async Task<ActionResult> GetAllBrand()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<BrandViewDto>>($"{ApiUrl}/brand");
            return View(response);
        }
        public async Task<ActionResult> GetAllImage()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ImageFileDto>>($"{ApiUrl}/imageFile");
            return View(response);
        }
        public async Task<ActionResult> GetAllProductVariant()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<VariantViewDto>>($"{ApiUrl}/productVariant");
            return View(response);
        }
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
