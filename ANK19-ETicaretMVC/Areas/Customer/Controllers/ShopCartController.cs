using ANK19_ETicaretMVC.Areas.Customer.Models.CustomerProfileViewModels;
using ANK19_ETicaretMVC.Enums;
using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.ShopCartViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Route("[area]/[controller]/[action]")]
    [Area("Customer")]

    public class ShopCartController : Controller
    {
        public async Task<IActionResult> Index()
        {
        
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);       
            var shopcartVm = await _httpClient.GetFromJsonAsync<ShopCartViewModel>("Customer/ShopCart/GetShopCart");    

            return View(shopcartVm);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(decimal totalPrice)
        {
            
            TempData["TotalPrice"] = totalPrice;
			HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
			var shopcartVm = await _httpClient.GetFromJsonAsync<ShopCartViewModel>("Customer/ShopCart/GetShopCart");
			return View(shopcartVm);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string userId)
        {         
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var response = await _httpClient.GetAsync($"Customer/CustomerOrder/CheckBasket?userId={userId}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart([FromForm] ItemRequest request)
        {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                // API'ye POST isteği
                var response = await _httpClient.PostAsJsonAsync("Customer/ShopCart/AddItemToCart", new
                {
                    request.Id,
                    request.Quantity
                    
                });
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Ürün sepete başarıyla eklendi.";
                    return RedirectToAction("Index");

                }


            if (request.ErrorFallBack != null)
            {
                return Redirect(request.ErrorFallBack);
            }



            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Ürün sepete eklenirken bir hata oluştu.");
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItemFromCart([FromForm] ItemRequest request)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            // API'ye POST isteği
            var response = await _httpClient.PostAsJsonAsync("Customer/ShopCart/RemoveItemFromCart", request);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Ürün sepetten başarıyla kaldırıldı.";
                return RedirectToAction("Index");

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Ürün sepetten kaldırılırken bir hata oluştu.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> IncrementItem([FromForm] ItemRequest request)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            // API'ye POST isteği
            var response = await _httpClient.PostAsJsonAsync("Customer/ShopCart/IncrementItem", request);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Ürün adeti başarıyla arttırıldı.";
                return RedirectToAction("Index");

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Ürün adeti arttırılırken bir hata oluştu.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DecrementItem([FromForm] ItemRequest request)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            // API'ye POST isteği
            var response = await _httpClient.PostAsJsonAsync("Customer/ShopCart/DecrementItem", request);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Ürün adeti başarıyla azaltıldı.";
                return RedirectToAction("Index");

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Ürün adeti azaltılırken bir hata oluştu.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EmptyCart()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            // API'ye POST isteği
            var response = await _httpClient.PostAsync("Customer/ShopCart/EmptyCart", null);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Ürünler sepetten başarıyla kaldırıldı.";
                return RedirectToAction("Index");

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Ürünler sepetten kqaldırılırken bir hata oluştu.");
            return RedirectToAction("Index");
        }











    }
}
