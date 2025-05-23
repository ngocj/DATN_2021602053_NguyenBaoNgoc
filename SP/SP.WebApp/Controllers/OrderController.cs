using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.UserDto;
using SP.Domain.Entity;
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
        public IActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(orderCreateDto);
            }

            // Lấy UserId từ claims nếu người dùng đã đăng nhập
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdString, out var userId))
            {
                // Nếu đã đăng nhập, dùng ID của họ
                orderCreateDto.UserId = userId;
            }
            else
            {
                var guestUserId = Guid.Parse("d74d8711-7654-43d9-a357-08dd9749ec40");
                orderCreateDto.UserId = guestUserId;

                var guestUser = await _httpClient.GetFromJsonAsync<UserViewDto>($"{ApiUrl1}User/{guestUserId}");

                if (string.IsNullOrWhiteSpace(orderCreateDto.UserName))
                {
                    orderCreateDto.UserName = guestUser?.UserName;
                }

            }
            orderCreateDto.Status = OrderStatus.Delivered; 

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}", orderCreateDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "❌ Failed to create order.");
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

    }
}
