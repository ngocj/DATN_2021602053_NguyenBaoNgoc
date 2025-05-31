using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CartDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.OrderDetailDto;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.ProductVariantDto;
using SP.Application.Dto.ProvinceDto;
using SP.Application.Dto.UserDto;
using SP.Domain.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
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

        public async Task<IActionResult> DetailOrder(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{id}");
            return View(response);
        }

        public async Task<IActionResult> BuyNowCheckout(int productVariantId, OrderCreateDto orderCreateDto, int quantity)
        {
            try
            {
                // 1. Kiểm tra số lượng hợp lệ
                if (quantity <= 0)
                {
                    TempData["Error"] = "❌ Số lượng đặt hàng phải lớn hơn 0";
                    return RedirectToAction("Index", "Home");
                }
                if (quantity > 5)
                {
                    TempData["Error"] = "❌ Số lượng đặt hàng không được vượt quá 5 sản phẩm";
                    return RedirectToAction("Index", "Home");
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

                // 4. Kiểm tra số lượng tồn kho
                if (quantity > productVariant.Quantity)
                {
                    TempData["Error"] = $"❌ Số lượng đặt hàng ({quantity}) vượt quá số lượng tồn kho ({productVariant.Quantity})";
                    return RedirectToAction("Index", "Home");
                }

                // 5. Lấy thông tin người dùng
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
                    Status = OrderStatus.Pending,
                    OrderDetails = orderDetails,
                    User = userInfo,
                    TotalPrice = productVariant.Price * quantity
                };

                // 8. Gửi các ViewData cần thiết
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

        public async Task<IActionResult> CartCheckout()
        {
            try
            {
                // 1. Kiểm tra token người dùng
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "❌ Vui lòng đăng nhập để tiếp tục";
                    return RedirectToAction("Login", "Auth");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "nameid");
                var userNameClaim = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId) || userNameClaim == null)
                {
                    TempData["Error"] = "❌ Phiên đăng nhập không hợp lệ";
                    return RedirectToAction("Login", "Auth");
                }

                // 2. Lấy thông tin người dùng
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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // 3. Lấy giỏ hàng
                var cartResponse = await _httpClient.GetAsync($"{ApiUrl1}cart/user/{userId}");
                if (!cartResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "❌ Không lấy được giỏ hàng";
                    return RedirectToAction("GetAllCartByUserId", "Cart");
                }
                var cartItems = await cartResponse.Content.ReadFromJsonAsync<List<CartViewDto>>();
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Error"] = "❌ Giỏ hàng của bạn đang trống";
                    return RedirectToAction("GetAllCartByUserId", "Cart");
                }

                // 4. Tạo OrderDetails
                var orderDetails = new List<OrderDetailCreateDto>();
                decimal totalPrice = 0;

                foreach (var item in cartItems)
                {
                    if (item.Quantity <= 0 || item.Quantity > 5)
                    {
                        TempData["Error"] = $"❌ Số lượng của sản phẩm (ID: {item.ProductVariantId}) không hợp lệ";
                        return RedirectToAction("GetAllCartByUserId", "Cart");
                    }

                    if (item.Quantity > item.ProductVariant.Quantity)
                    {
                        TempData["Error"] = $"❌ Số lượng đặt hàng ({item.Quantity}) của sản phẩm {item.ProductVariant.ProductName} vượt quá tồn kho ({item.ProductVariant.Quantity})";
                        return RedirectToAction("GetAllCartByUserId", "Cart");
                    }

                    orderDetails.Add(new OrderDetailCreateDto
                    {
                        ProductVariantId = item.ProductVariantId,
                        Quantity = item.Quantity,
                        Price = item.ProductVariant.Price,
                        ProductVariant = item.ProductVariant
                    });

                    totalPrice += item.ProductVariant.Price * item.Quantity;
                }

                // 5. Tạo OrderCreateDto
                var orderDto = new OrderCreateDto
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    UserName = userInfo.UserName,
                    PhoneNumber = userInfo.PhoneNumber,
                    AddressDetail = userInfo.AddressDetail,
                    Status = OrderStatus.Pending,
                    OrderDetails = orderDetails,
                    User = userInfo,
                    TotalPrice = totalPrice
                };

                // 6. Gửi ViewData
                var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryViewDto>>($"{ApiUrl1}category");
                ViewBag.Categories = categories != null ? new SelectList(categories, "Id", "CategoryName") : null;

                var provinces = await _httpClient.GetFromJsonAsync<IEnumerable<Province>>($"{ApiUrl1}Address/provinces");
                ViewBag.Provinces = provinces != null ? new SelectList(provinces, "Id", "Name") : null;

                // 7. Trả về View
                return View(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckOut GET: {ex.Message}");
                TempData["Error"] = "❌ Đã xảy ra lỗi khi xử lý yêu cầu";
                return RedirectToAction("GetAllCartByUserId", "Cart");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CartCheckout(OrderCreateDto orderCreateDto)
        {
            const int MaxQuantityPerItem = 5;

            try
            {
                // 1. Lấy token người dùng từ session
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "❌ Vui lòng đăng nhập để tiếp tục";
                    return RedirectToAction("Login", "Auth");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "nameid");
                var userNameClaim = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId) || userNameClaim == null)
                {
                    TempData["Error"] = "❌ Phiên đăng nhập không hợp lệ";
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // 2. Lấy giỏ hàng của user
                var cartResponse = await _httpClient.GetAsync($"{ApiUrl1}cart/user/{userId}");
                if (!cartResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "❌ Không lấy được giỏ hàng";
                    return RedirectToAction("CartCheckout", "Order");
                }

                var cartItems = await cartResponse.Content.ReadFromJsonAsync<List<CartViewDto>>();
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Error"] = "❌ Giỏ hàng của bạn đang trống";
                    return RedirectToAction("CartCheckout", "Order");
                }

                // 3. Tạo ID đơn hàng mới, gán thông tin người dùng và trạng thái
                orderCreateDto.Id = Guid.NewGuid();
                orderCreateDto.UserId = userId;
                orderCreateDto.UserName = userNameClaim.Value;
                orderCreateDto.Status = OrderStatus.Pending;

                // 4. Chuẩn bị danh sách OrderDetails và tính tổng tiền
                var orderDetails = new List<OrderDetailCreateDto>();
                decimal totalPrice = 0;

                foreach (var item in cartItems)
                {
                    if (item.Quantity <= 0 || item.Quantity > MaxQuantityPerItem)
                    {
                        TempData["Error"] = $"❌ Số lượng của sản phẩm (ID: {item.ProductVariantId}) không hợp lệ";
                        return RedirectToAction("CartCheckout", "Order");
                    }

                    if (item.Quantity > item.ProductVariant.Quantity)
                    {
                        TempData["Error"] = $"❌ Số lượng đặt hàng ({item.Quantity}) của sản phẩm {item.ProductVariant.ProductName} vượt quá tồn kho ({item.ProductVariant.Quantity})";
                        return RedirectToAction("CartCheckout", "Order");
                    }

                    orderDetails.Add(new OrderDetailCreateDto
                    {
                        OrderId = orderCreateDto.Id,
                        ProductVariantId = item.ProductVariantId,
                        Quantity = item.Quantity,
                        Price = item.ProductVariant.Price
                    });

                    totalPrice += item.ProductVariant.Price * item.Quantity;
                }

                orderCreateDto.TotalPrice = totalPrice;
                orderCreateDto.OrderDetails = orderDetails;

                // 5. Gửi request tạo đơn hàng
                var orderResponse = await _httpClient.PostAsJsonAsync(ApiUrl, orderCreateDto);
                if (!orderResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "❌ Tạo đơn hàng thất bại. Vui lòng thử lại";
                    return RedirectToAction("CartCheckout", "Order");
                }

                // 6. Xóa các sản phẩm trong giỏ sau khi đặt thành công
                foreach (var item in cartItems)
                {
                    var deleteResponse = await _httpClient.DeleteAsync($"{ApiUrl1}cart/{userIdClaim.Value}/{item.ProductVariantId}");
                    if (!deleteResponse.IsSuccessStatusCode)
                    {
                        // Có thể log lỗi, không chặn quá trình
                        Console.WriteLine($"Không thể xóa sản phẩm {item.ProductVariantId} khỏi giỏ hàng.");
                    }
                }

                TempData["Success"] = "🎉 Đặt hàng thành công! Đơn hàng đang được xử lý";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CartCheckout Error] {ex}");
                TempData["Error"] = "❌ Đã xảy ra lỗi khi đặt hàng. Vui lòng liên hệ hỗ trợ";
                return RedirectToAction("CartCheckout", "Order");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Payment(int productVariantId, OrderCreateDto orderCreateDto, int quantity)
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
                    return RedirectToAction("Index", "Home");
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
        }

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
   
        public async Task<IActionResult> UpdateOrder(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<OrderUpdateDto>($"{ApiUrl}/{id}");
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(OrderUpdateDto orderUpdate)
        {
            var token = HttpContext.Session.GetString("JwtToken");   
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "nameid");
            var userNameClaim = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid EmployeeId) || userNameClaim == null)
                return RedirectToAction("Login", "Auth");

            string userNameFromToken = userNameClaim.Value;

            if (!ModelState.IsValid)
            {
                return View(orderUpdate);
            }

            orderUpdate.EmployeeId = EmployeeId;      
            

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, orderUpdate);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllOrder", "Employee");
            }

            ModelState.AddModelError("", "Cập nhật không thành công.");
            return View(orderUpdate);
        }

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
        public async Task<IActionResult> CancelOrder(Guid id)
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
            return RedirectToAction("OrderHistory", "Order");

        }
        public async Task<IActionResult> OrderHistory()
        {
            var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryViewDto>>($"{ApiUrl1}category");
            ViewBag.Categories = categories != null ? new SelectList(categories, "Id", "CategoryName") : null;
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "nameid");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return RedirectToAction("Login", "Auth");

            var orders = await _httpClient.GetFromJsonAsync<List<OrderViewDto>>($"{ApiUrl}/user/{userId}");

            return View(orders);
        }


       




    }
}
