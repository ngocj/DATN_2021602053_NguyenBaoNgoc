using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.OrderDetailDto;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.ProductVariantDto;
using SP.Application.Dto.ProvinceDto;
using SP.Application.Dto.UserDto;
using SP.Domain.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace SP.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/order";
        private const string ApiUrl1 = "https://localhost:7131/api/";
        private HttpClient _httpClient;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        // get order by id
        public async Task<IActionResult> DetailOrder(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{id}");
            return View(response);
        }
        // create order
        public async Task<IActionResult> ByNow(int productVariantId, OrderCreateDto orderCreateDto, int quantity)
        {
            try
            {
                // 1. Kiểm tra số lượng hợp lệ
                if (quantity <= 0)
                {
                    TempData["Error"] = "❌ Số lượng đặt hàng phải lớn hơn 0";
                    return RedirectToAction("Index");
                }

                // 2. Kiểm tra token người dùng
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login", "Auth");

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "nameid");
                var userNameClaim = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId) || userNameClaim == null)
                    return RedirectToAction("Login", "Auth");

                string userNameFromToken = userNameClaim.Value;

                // 3. Lấy thông tin sản phẩm
                var productResponse = await _httpClient.GetAsync($"{ApiUrl1}productvariant/{productVariantId}");
                if (!productResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "❌ Không tìm thấy sản phẩm";
                    return RedirectToAction("Index", "Home");
                }

                var productVariant = await productResponse.Content.ReadFromJsonAsync<VariantViewDto>();
                if (productVariant == null)
                {
                    TempData["Error"] = "❌ Thông tin sản phẩm không hợp lệ";
                    return RedirectToAction("Index", "Home");
                }

                // 4. Lấy thông tin người dùng
                var userResponse = await _httpClient.GetAsync($"{ApiUrl1}user/{userId}");
                if (!userResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "❌ Không lấy được thông tin người dùng";
                    return RedirectToAction("Login", "Auth");
                }

                var userInfo = await userResponse.Content.ReadFromJsonAsync<UserViewDto>();
                if (userInfo == null)
                {
                    TempData["Error"] = "❌ Thông tin người dùng không hợp lệ";
                    return RedirectToAction("Login", "Auth");
                }

                // 5. Lấy DistrictId và ProvinceId từ WardId
                var wardResponse = await _httpClient.GetAsync($"{ApiUrl1}address/ward/1");
                if (!wardResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "❌ Không lấy được thông tin địa chỉ";
                    return RedirectToAction("Index", "Home");
                }

                var wardInfo = await wardResponse.Content.ReadFromJsonAsync<WardViewDto>();
                if (wardInfo == null || wardInfo.District == null || wardInfo.District.Province == null)
                {
                    TempData["Error"] = "❌ Dữ liệu địa chỉ không đầy đủ";
                    return RedirectToAction("Index", "Home");
                }


                // 6. Chuẩn bị OrderDetails
                var orderDetails = new List<OrderDetailCreateDto>
        {
            new OrderDetailCreateDto
            {
                ProductVariantId = productVariantId,
                Quantity = quantity,
                Price = productVariant.Price,
                ProductVariant = productVariant
            }
        };

                // 7. Tạo mới OrderCreateDto
                var orderDto = new OrderCreateDto
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    UserName = userInfo.UserName,
                    PhoneNumber = userInfo.PhoneNumber,
                    AddressDetail = userInfo.AddressDetail,
                    WardId = userInfo.WardId,
                    DistrictId = wardInfo.District.Id,
                    ProvinceId = wardInfo.District.Province.Id,
                    Status = OrderStatus.Pending,
                    OrderDetails = orderDetails,
                    User = userInfo,
                    TotalPrice = productVariant.Price * quantity
                };

                // 8. Gửi các ViewData cần thiết (nếu dùng View có chọn danh mục hoặc địa chỉ)
                var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryViewDto>>($"{ApiUrl1}category");
                ViewBag.Categories = categories != null ? new SelectList(categories, "Id", "CategoryName") : null;

                var provinces = await _httpClient.GetFromJsonAsync<IEnumerable<Province>>($"{ApiUrl1}Address/provinces");
                ViewBag.Provinces = provinces != null ? new SelectList(provinces, "Id", "Name") : null;

                // 9. Trả về View thanh toán với thông tin đã có
                return View(orderDto);
            }
            catch (Exception)
            {
                TempData["Error"] = "❌ Đã xảy ra lỗi khi xử lý yêu cầu";
                return RedirectToAction("Index", "Home");
            }
        }


        /*        [HttpPost]
                public async Task<IActionResult> ByNow(int productVariantId, OrderCreateDto orderCreateDto, int quantity)
                {
                    try
                    {
                        // 1. Kiểm tra số lượng hợp lệ
                        if (quantity <= 0)
                        {
                            TempData["Error"] = "❌ Số lượng đặt hàng phải lớn hơn 0";
                            return RedirectToAction("Index");
                        }

                        // 2. Kiểm tra token người dùng
                        var token = HttpContext.Session.GetString("JwtToken");
                        if (string.IsNullOrEmpty(token))
                            return RedirectToAction("Login", "Auth");

                        var handler = new JwtSecurityTokenHandler();
                        var jwt = handler.ReadJwtToken(token);

                        var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "nameid");
                        var userNameClaim = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");

                        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId) || userNameClaim == null)
                            return RedirectToAction("Login", "Auth");

                        string userNameFromToken = userNameClaim.Value;

                        // 3. Gán thông tin người dùng vào đơn hàng
                        orderCreateDto.Id = Guid.NewGuid();
                        orderCreateDto.UserId = userId;
                        orderCreateDto.UserName = userNameFromToken;
                        orderCreateDto.Status = OrderStatus.Pending;

                        // 4. Lấy thông tin sản phẩm biến thể
                        var productResponse = await _httpClient.GetAsync($"{ApiUrl1}productvariant/{productVariantId}");
                        if (!productResponse.IsSuccessStatusCode)
                        {
                            TempData["Error"] = "❌ Không tìm thấy sản phẩm";
                            return RedirectToAction("Index","Home");
                        }

                        var productVariant = await productResponse.Content.ReadFromJsonAsync<VariantViewDto>();
                        if (productVariant == null)
                        {
                            TempData["Error"] = "❌ Thông tin sản phẩm không hợp lệ";
                            return RedirectToAction("Index", "Home");
                        }

                        if (productVariant.Quantity < quantity)
                        {
                            TempData["Error"] = $"❌ Chỉ còn {productVariant.Quantity} sản phẩm trong kho";
                            return RedirectToAction("Index", "Home");
                        }

                        // 5. Cập nhật đơn hàng và chi tiết đơn hàng
                        orderCreateDto.TotalPrice = productVariant.Price * quantity;
                        orderCreateDto.OrderDetails = new List<OrderDetailCreateDto>
                {
                    new OrderDetailCreateDto
                    {
                        OrderId = orderCreateDto.Id,
                        ProductVariantId = productVariantId,
                        Price = productVariant.Price,
                        Quantity = quantity
                    }
                };

                        // 6. Gửi request tạo đơn hàng
                        var orderResponse = await _httpClient.PostAsJsonAsync(ApiUrl, orderCreateDto);
                        if (!orderResponse.IsSuccessStatusCode)
                        {
                            TempData["Error"] = "❌ Tạo đơn hàng thất bại. Vui lòng thử lại";
                            return RedirectToAction("Index", "Home");
                        }

                        TempData["Success"] = "🎉 Đặt hàng thành công! Đơn hàng đang được xử lý";
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception)
                    {
                        TempData["Error"] = "❌ Đã xảy ra lỗi khi đặt hàng. Vui lòng liên hệ hỗ trợ";
                        return RedirectToAction("Index", "Home");
                    }
                }*/

        // Thêm API endpoints để lấy quận/huyện và phường/xã
        [HttpGet]
        public async Task<IActionResult> GetDistrictsByProvince(int provinceId)
        {
            var districts = await _httpClient.GetFromJsonAsync<IEnumerable<DistrictViewDto>>($"{ApiUrl1}Address/districts/{provinceId}");
            Console.WriteLine($"Districts count: {districts?.Count()}"); 
            return Json(districts);
        }

        [HttpGet]
        public async Task<IActionResult> GetWardsByDistrict(int districtId)
        {
            var wards = await _httpClient.GetFromJsonAsync<IEnumerable<WardViewDto>>($"{ApiUrl1}Address/wards/{districtId}");                      
            return Json(wards);
        }


        


        
        // update order
        public async Task<IActionResult> UpdateOrder(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<OrderUpdateDto>($"{ApiUrl}/{id}");
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(OrderUpdateDto orderUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(orderUpdate);
            }
            var guestUser = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{orderUpdate.Id}");

            orderUpdate.UserId = guestUser.UserId;
            orderUpdate.EmployeeId = guestUser.EmployeeId;

            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}", orderUpdate);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllOrder", "Employee");
            }
            ModelState.AddModelError("", "❌ Failed to update order.");
            return View(orderUpdate);
        }
        // delete order
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa đơn hàng thành công.";
            }
            else
            {
                TempData["Error"] = "Xóa đơn hàng không thành công.";
            }
            return RedirectToAction("GetAllOrder", "Manager");

        }
 
    }
}
