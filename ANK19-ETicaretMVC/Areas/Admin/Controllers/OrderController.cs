using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.OrderViewModels;
using DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<List<OrderListViewModel>>("Admin/Order/GetOrders"));
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromForm] int id, [FromForm] OrderStatus status)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            // API'ye gönderilecek JSON verisi
            var requestBody = new
            {
                Id = id,
                Status = status
            };

            // API'ye POST isteği
            var response = await _httpClient.PostAsJsonAsync("Admin/Order/UpdateOrderStatus", requestBody);

            // API isteği başarılı mı kontrol et
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sipariş durumu başarıyla güncellendi.";
                return RedirectToAction("Index");

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Sipariş durumu güncellenirken bir hata oluştu.");
            return RedirectToAction("Index");
        }

        // GET: /Order/GetOrderById
        [HttpGet]
        public async Task<IActionResult> GetOrderById(int? searchId)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            // Eğer searchId boşsa, hata döndür
            if (!searchId.HasValue)
            {
                return RedirectToAction("Index"); // veya uygun bir view'ye yönlendirin
            }

            ViewData["SearchId"] = searchId.Value; // Kullanıcının girdiği ID'yi ViewData'ya ekliyoruz

            try
            {
                // API'ye GET isteği gönderiyoruz
                var order = await _httpClient.GetFromJsonAsync<OrderListViewModel>(
                    $"Admin/Order/GetOrderById?id={searchId.Value}"
                );


                if (order == null)
                {
                    TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                    return RedirectToAction("Index"); // veya uygun bir view'ye yönlendirin
                }

                // Sipariş bulundu, veriyi View'a gönderiyoruz
                return View("Index", new List<OrderListViewModel>() { order });

            }
            catch
            {
                TempData["ErrorMessage"] = "Aradığınız sipariş numarası ile sipariş bulunamadı.";
                return View("Index", new List<OrderListViewModel>());
            }

        }

    }
}
