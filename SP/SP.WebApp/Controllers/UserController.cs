using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.UserDto;
using System.IdentityModel.Tokens.Jwt;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {
            var result = await _httpClient.PostAsJsonAsync(ApiUrl, userCreateDto);

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "User created successfully !";

            }
            else
            {
                TempData["Error"] = "Failed to create user !";
            }
            return RedirectToAction("GetAllUser", "Admin");
        }

        /* public async Task<IActionResult> UpdateUser(int id)
         {
             var response = await _httpClient.GetFromJsonAsync<UserUpdateDto>($"{ApiUrl}/{id}");
             return View(response);
         }

         [HttpPost]
         public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
         {
             var token = HttpContext.Session.GetString("JwtToken");
             _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
             var response = await _httpClient.PutAsJsonAsync(ApiUrl, userUpdateDto);

             if (!response.IsSuccessStatusCode)
             {
                 TempData["Error"] = "Failed to update user";
             }
             else
             {
                 TempData["Success"] = "User updated successfully!";
             }
             return RedirectToAction("GetAllUser", "Admin");
         }*/

        /*  public async Task<IActionResult> DeleteUser(int id)
          {
              var token = HttpContext.Session.GetString("JwtToken");
              _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
              var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");

              if (!response.IsSuccessStatusCode)
              {
                  TempData["Error"] = "Cannot delete this user because it contains blog posts!";
              }
              else
              {

                  TempData["Success"] = "User deleted successfully!";
              }

              return RedirectToAction("GetAllUser", "Admin");
          }*/

        public async Task<IActionResult> GetUserById(int id)
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
    }
}
