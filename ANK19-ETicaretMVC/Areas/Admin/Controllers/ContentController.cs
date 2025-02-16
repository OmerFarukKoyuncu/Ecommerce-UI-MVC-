using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.ContentViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Admin")]
    public class ContentController: Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View("Index",await _httpClient.GetFromJsonAsync<List<ContentViewModel>>("Admin/Content/GetContents"));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ContentViewModel content)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            // API'ye POST isteği
            var response = await _httpClient.PostAsJsonAsync("Admin/Content/UpdateContent", content);

            // API isteği başarılı mı kontrol et
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "İçerik başarıyla güncellendi.";
                return await Index();

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "İçerik güncellenirken bir hata oluştu.");
            return await Index();
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<ContentViewModel>($"Admin/Content/GetContentById?id={id}"));
        }

    }
}
