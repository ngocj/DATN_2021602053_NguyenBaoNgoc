using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SP.Application.Dto.EmployeeDto;
using SP.Application.Dto.OrderDto;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace SP.WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/employee";
        private const string ApiUrl1 = "https://localhost:7131/api";
        private HttpClient _httpClient;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        [Authorize(Roles = "Manager")]
        public IActionResult CreateEmployee()
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Quản trị viên", Value = "1" },
                new SelectListItem { Text = "Quản lý", Value = "2" },
                new SelectListItem { Text = "Nhân viên", Value = "3" },
                new SelectListItem { Text = "Khách hàng", Value = "4" }
            };
            return View();
        }
       
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateEmployee(EmployeeCreateDto employeeCreateDto)
        {

            if (!ModelState.IsValid)
            {
                return View(employeeCreateDto);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}", employeeCreateDto);

            if (!response.IsSuccessStatusCode)
            {
                // Đọc nội dung lỗi từ response
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    try
                    {
                        // Deserialize JSON lỗi từ API: { "field": "Email", "message": "Email đã tồn tại." }
                        var errorObj = JsonSerializer.Deserialize<Dictionary<string, string>>(content);

                        if (errorObj != null && errorObj.ContainsKey("field") && errorObj.ContainsKey("message"))
                        {
                            ModelState.AddModelError(errorObj["field"], $"❌ {errorObj["message"]}");
                        }
                        else
                        {
                            ModelState.AddModelError("", "❌ Lỗi không xác định.");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "❌ Không thể đọc lỗi từ server.");
                    }

                    return View(employeeCreateDto);
                }

                TempData["Error"] = "❌ Thêm nhân viên dùng thất bại.";
                return View(employeeCreateDto);
            }

            TempData["Success"] = "🎉 Thêm nhân viên thành công!";
            return RedirectToAction("GetAllEmployee", "Manager");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateEmployee(Guid id)
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Quản trị viên", Value = "1" },
                new SelectListItem { Text = "Quản lý", Value = "2" },
                new SelectListItem { Text = "Nhân viên", Value = "3" },
                new SelectListItem { Text = "Khách hàng", Value = "4" }
            };
            var response = await _httpClient.GetFromJsonAsync<EmployeeUpdateDto>($"{ApiUrl}/{id}");
            return View(response);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateEmployee(EmployeeUpdateDto EmployeeUpdateDto)
        {
            // Lấy thông tin nhân viên hiện tại từ API bằng ID
            var existingEmployee = await _httpClient.GetFromJsonAsync<EmployeeUpdateDto>($"{ApiUrl}/{EmployeeUpdateDto.Id}");

            if (existingEmployee == null)
            {
                TempData["Error"] = "Không tìm thấy nhân viên.";
                return RedirectToAction("GetAllEmployee", "Manager");
            }

            // Gửi yêu cầu cập nhật
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, EmployeeUpdateDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật nhân viên thành công!";
            }
            else
            {
                TempData["Error"] = "Cập nhật nhân viên thất bại.";
            }

            return RedirectToAction("GetAllEmployee", "Manager");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            /*var token = HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);*/
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa nhân viên thành công!";
            }
            else
            {
                TempData["Error"] = "Xóa nhân viên thất bại.";
            }
            return RedirectToAction("GetAllEmployee", "Manager");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            var isLoggedIn = !string.IsNullOrEmpty(token);
            bool isManager = false;

            if (isLoggedIn)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
                if (roleClaim?.Value == "Manager") isManager = true;
            }
            ViewBag.IsManager = isManager;
            var response = await _httpClient.GetFromJsonAsync<EmployeeViewDto>($"{ApiUrl}/{id}");
            return View(response);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DetailEmployee(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<EmployeeViewDto>($"{ApiUrl}/{id}");
            return View(response);
        }

        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> GetAllOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrderViewDto>>($"{ApiUrl1}/order");
            return View(response);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
