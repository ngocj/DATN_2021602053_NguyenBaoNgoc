using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.OrderDetailDto;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.UserDto;
using SP.Domain.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        public IActionResult BuyNow()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(OrderCreateDto orderCreateDto)
        {
            // Lấy JWT từ session
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            // Giải mã JWT để lấy userId
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");
            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Gán userId và userName vào DTO
            orderCreateDto.UserId = userId;
            orderCreateDto.UserName = claim?.Value;
            orderCreateDto.Status = OrderStatus.Pending;
            orderCreateDto.TotalPrice = 100;
            orderCreateDto.EmployeeId = Guid.NewGuid();
            orderCreateDto.EmployeeName = "ngoc";

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, orderCreateDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "📦 Đặt hàng thành công.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "❌ Đặt hàng thất bại.";
            return View(orderCreateDto);
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

      
       

        //payment
        public async Task<IActionResult> Payment(Guid orderId)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{"1144423C-1A45-476D-8A9B-ABDD3AE6C666"}");
            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index", "Product");
            }
            return View(order);
        }













    }
}
