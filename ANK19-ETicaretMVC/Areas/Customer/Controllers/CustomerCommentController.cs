using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Customer")]
    public class CustomerCommentController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddComment([FromForm] int productId, [FromForm] string? content, [FromForm] int? productRate)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            // Request için gönderilecek veriyi oluştur
            var request = new
            {
                Content = content,  
                ProductRate = productRate,
            };

            // POST isteği gönder

            var response = await _httpClient.PostAsJsonAsync($"Customer/CustomerComment/AddComment?productId={productId}", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Yorum başarılı bir şekilde eklendi.";
                
                return RedirectToAction("Index", "CustomerOrder"); // Index veya başka bir sayfaya yönlendirin
            }
            else
            {

               
                TempData["ErrorMessage"] = "Lütfen puanlama ile birlikte yorum da yapınız.";
               
                return RedirectToAction("Index","CustomerOrder"); // Hata durumunda da bir yönlendirme yapabilirsiniz
            }
        }
    }
}
