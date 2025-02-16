using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Seller.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Seller")]
    public class CommentController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<List<CommentManagerViewModel>>("Seller/Comment/GetComments"));
        }

        [HttpPost]
        public async Task<IActionResult> ReplyToComment([FromForm] int commentId, [FromForm] string? replyText, [FromForm] string? returnUrl)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            // Request için gönderilecek veriyi oluştur
            var request = new
            {
                ReplyText = replyText
            };

            // POST isteği gönder

            var response = await _httpClient.PostAsJsonAsync($"Seller/Comment/ReplyToComment/{commentId}/reply", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Yorum başarılı bir şekilde yanıtlandı.";
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index"); // Index veya başka bir sayfaya yönlendirin
            }
            else
            {

                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Hata oluştu: {errorMessage}";
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index"); // Hata durumunda da bir yönlendirme yapabilirsiniz
            }
        }
    }
}
