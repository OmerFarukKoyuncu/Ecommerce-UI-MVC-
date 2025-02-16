using ANK19_ETicaretMVC.Areas.Admin.Enums;
using ANK19_ETicaretMVC.Areas.Admin.Models.AdminViewModels;
using ANK19_ETicaretMVC.Areas.Admin.Models.UserViewModels;
using ANK19_ETicaretMVC.Areas.Customer.Models.RefundChangeViewModels;
using ANK19_ETicaretMVC.Areas.Seller.Models;
using ANK19_ETicaretMVC.Areas.Seller.Models.OrderViewModels;
using ANK19_ETicaretMVC.Areas.Seller.Models.RefundViewModels;
using ANK19_ETicaretMVC.Enums;
using ANK19_ETicaretMVC.Helpers;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;


namespace ANK19_ETicaretMVC.Areas.Seller.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Seller")]	
	public class SellerOrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> SellerOrders()
        {
			HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
			var token = HttpContext.Session.GetString("JwtToken"); // Token'ı al
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token); // Token'ı çöz

			// Sub claim'ini al
			var sellerIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

			if (sellerIdClaim == null)
			{
				return Unauthorized("Seller ID bulunamadı.");
			}
			var sellerId = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerIdClaim}");


            var orders = await _httpClient.GetFromJsonAsync<List<OrderSellerViewModel>>($"SellerOrder/GetOrdersBySellerId?sellerId={sellerId}");

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetSellerOrderById(int orderId)
        {
			HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
			var token = HttpContext.Session.GetString("JwtToken"); // Token'ı al
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token); // Token'ı çöz

			// Sub claim'ini al
			var sellerIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

			if (sellerIdClaim == null)
			{
				return Unauthorized("Seller ID bulunamadı.");
			}
			var sellerId = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerIdClaim}");

			var order = await _httpClient.GetFromJsonAsync<OrderSellerViewModel>($"SellerOrder/GetOrderBySellerId?sellerId={sellerId}&orderId={orderId}");
                 //SellerOrder/GetOrderBySellerId?sellerId=1&orderId=2

            return View(order);
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
            var response = await _httpClient.PostAsJsonAsync("SellerOrder/UpdateOrderStatusForSeller", requestBody);
                

            // API isteği başarılı mı kontrol et
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sipariş durumu başarıyla güncellendi.";
                return RedirectToAction("SellerOrders");

            }

            // Hata durumunda uygun bir işlem
            ModelState.AddModelError("", "Sipariş durumu güncellenirken bir hata oluştu.");
            return RedirectToAction("SellerOrders");
        }

		[HttpGet]
		public async Task<IActionResult> RefundsChanges()
		{
			HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
			var token = HttpContext.Session.GetString("JwtToken"); // Token'ı al
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token); // Token'ı çöz

			// Sub claim'ini al
			var sellerIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

			if (sellerIdClaim == null)
			{
				return Unauthorized("Seller ID bulunamadı.");
			}
			var sellerId = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerIdClaim}");

            if (TempData["DeleteMessage"] != null)
            {
                ViewBag.DeleteMessage = TempData["DeleteMessage"];
            }
            if (TempData["RequestApproveMessage"] != null)
            {
                ViewBag.RequestApproveMessage = TempData["RequestApproveMessage"];
            }
            var refundsDto = await _httpClient.GetFromJsonAsync<List<GetRefundChangesViewModel>>($"SellerOrder/GetRefundsChangesFoSeller?sellerId={sellerId}");

			return View(refundsDto);
		}

        [HttpPost]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var refundsDto = await _httpClient.DeleteAsync($"SellerOrder/DeleteRequest?requestId={requestId}");
            TempData["DeleteMessage"] = "İstek silindi.";
            return RedirectToAction("RefundsChanges");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRefundChange(string userId,int productId,RequestType request, int requestId)
        {
            ApproveRefundChangeForSellerVm requestBody = new() 
            {
                userId = userId,
                productId = productId,
                Request = request
            };
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
			//var refundsDto = await _httpClient.PostAsJsonAsync($"SellerOrder/ApproveRequest", requestBody);
			var refundsDto = await _httpClient.DeleteAsync($"SellerOrder/DeleteRequest?requestId={requestId}");
			TempData["RequestApproveMessage"] = "İstek onaylandı.";
            return RedirectToAction("RefundsChanges");
        }
        
    }
}
