using ANK19_ETicaretMVC.Areas.Admin.Enums;
using ANK19_ETicaretMVC.Areas.Customer.Models.RefundChangeViewModels;
using ANK19_ETicaretMVC.Areas.Seller.Models.SellerReportsViewModels;
using ANK19_ETicaretMVC.Enums;
using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
	[Route("[area]/[controller]/[action]/{id?}")]
	[Area("Customer")]
	public class ProductRefundChangeController : Controller
	{
		public async Task<IActionResult> Index()
		{
			HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
			var token = HttpContext.Session.GetString("JwtToken"); // Token'ı al
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token); // Token'ı çöz

			// Sub claim'ini al
			var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

			if (userId == null)
			{
				return Unauthorized("User ID bulunamadı.");
			}

			if (TempData["ErrorMessage"] != null)
			{
				ViewBag.ErrorMessage = TempData["ErrorMessage"];
			}
			if (TempData["SuccessMessage"] != null)
			{
				ViewBag.SuccessMessage = TempData["SuccessMessage"];
			}

			List<ProductsViewModelForRefundChange> productsforrefund = new();

			try
			{
				productsforrefund = await _httpClient.GetFromJsonAsync<List<ProductsViewModelForRefundChange>>($"Customer/CustomerRefund/GetOrdersByUserId?userId={userId}");
			}
			catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				
				ViewBag.NoRefundProductsMessage = "İade ve değişim talebinde bulunacak ürün yoktur.";
			}

			return View(productsforrefund);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRefundRequest(string userId, int productId, int requestType)
		{
			if (requestType == 0)
			{
				TempData["ErrorMessage"] = "İade Türünüzü Seçiniz";
				return RedirectToAction("Index");
			}

			HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);


			var apiUrl = $"Customer/CustomerRefund/AddRefundChange";


			var requestBody = new
			{
				userId = userId,
				productId = productId,
				request = (RequestType)requestType
			};


			var response = await _httpClient.PostAsJsonAsync(apiUrl, requestBody);

			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "İade talebi başarıyla oluşturuldu.";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["ErrorMessage"] = "Talebiniz zaten oluşturulmuştur.";
				return RedirectToAction("Index");
			}
		}

	}
}
