using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.FeedbackDto;

namespace SP.WebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private const string ApiUrl = "https://localhost:7131/api/feedback";
        private HttpClient _httpClient;

        public FeedbackController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<ActionResult> DeleteFeedback(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa đánh giá  thành công.";
            }
            else
            {
                TempData["Error"] = "Xóa đánh giá không thành công.";
            }
            return RedirectToAction("GetAllFeedback", "Admin");
        }
   
        public async Task<IActionResult> DetailFeedback(int id)
        {
            var feedback = await _httpClient.GetFromJsonAsync<FeedbackViewDto>($"{ApiUrl}/{id}");
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }
        
    }
}
