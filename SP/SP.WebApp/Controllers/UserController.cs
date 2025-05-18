using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SP.Application.Dto.UserDto;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace SP.WebApp.Controllers
{

    public class UserController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/user";
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClient)
        {

            _httpClient = httpClient.CreateClient();
        }
        public IActionResult CreateUser()
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Quản trị viên", Value = "1" },
                new SelectListItem { Text = "Quản lý", Value = "2" },
                new SelectListItem { Text = "Khách hàng", Value = "4" }
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateDto);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}", userCreateDto);

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

                    return View(userCreateDto);
                }

                TempData["Error"] = "❌ Thêm người dùng thất bại.";
                return View(userCreateDto);
            }

            TempData["Success"] = "🎉 Thêm người dùng thành công!";
            return RedirectToAction("GetAllUser", "Admin");
        }

        public async Task<IActionResult> UpdateUser(Guid id)
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Quản trị viên", Value = "1" },
                new SelectListItem { Text = "Quản lý", Value = "2" },
                new SelectListItem { Text = "Khách hàng", Value = "4" }
            };
            var response = await _httpClient.GetFromJsonAsync<UserUpdateDto>($"{ApiUrl}/{id}");
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            // Lấy thông tin người dùng hiện tại từ API bằng ID
            var existingUser = await _httpClient.GetFromJsonAsync<UserUpdateDto>($"{ApiUrl}/{userUpdateDto.Id}");

            if (existingUser == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("GetAllUser", "Admin");
            }

            // Gộp lại thông tin đầy đủ (giữ nguyên các field không thay đổi)
            userUpdateDto.Email = existingUser.Email;
            userUpdateDto.UserName = existingUser.UserName;
            userUpdateDto.Password = existingUser.Password;

            // Gửi yêu cầu cập nhật
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, userUpdateDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật người dùng thành công!";
            }
            else
            {
                TempData["Error"] = "Cập nhật người dùng thất bại.";
            }

            return RedirectToAction("GetAllUser", "Admin");
        }


        public async Task<IActionResult> DeleteUser(Guid id)
        {
            /*var token = HttpContext.Session.GetString("JwtToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);*/
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa người dùng thành công!";
            }
            else
            {
                TempData["Error"] = "Xóa người dùng thất bại.";
            }
            return RedirectToAction("GetAllUser", "Admin");          
        }

        public async Task<IActionResult> GetUserById(Guid id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            var isLoggedIn = !string.IsNullOrEmpty(token);
            bool isAdmin = false;

            if (isLoggedIn)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
                if (roleClaim?.Value == "Admin") isAdmin = true;
            }
            ViewBag.IsAdmin = isAdmin;
            var response = await _httpClient.GetFromJsonAsync<UserViewDto>($"{ApiUrl}/{id}");
            return View(response);
        }
        public async Task<IActionResult> DetailUser(Guid id)
        {     
            var response = await _httpClient.GetFromJsonAsync<UserViewDto>($"{ApiUrl}/{id}");
            return View(response);
        }

    }
}
