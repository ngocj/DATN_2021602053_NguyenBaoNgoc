using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.LoginDto;
using SP.Application.Dto.UserDto;

namespace SP.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/auth";
        private const string ApiUrlUser = "https://localhost:7131/api/user";
        private HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }
        // login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewDto loginViewDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewDto);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/login", loginViewDto);

            if (!response.IsSuccessStatusCode)
            {

                ModelState.AddModelError("", "❌ Incorrect email or password.");
                return View(loginViewDto);
            }

            var token = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("JwtToken", token);

            return RedirectToAction("Index", "Home");
        }
        // forgot password
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Index", "Home");
        }


        // register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto userViewDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewDto);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/register", userViewDto);

            // check email already exists
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("", "❌ Email đã tồn tại.");
                return View(userViewDto);
            }         
            // check phone number already exists
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                ModelState.AddModelError("", "❌ Số điện thoại đã tồn tại.");
                return View(userViewDto);
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi tạo tài khoản.";
                return View(userViewDto);
            }
        
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Tài khoản đã được tạo thành công!";


            }

            return RedirectToAction("Login", "Auth");
        }
    }
}
