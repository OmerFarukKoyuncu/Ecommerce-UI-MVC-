using ANK19_ETicaretMVC.Areas.Admin.Models.OrderReport;
using ANK19_ETicaretMVC.Areas.Seller.Models.SellerReportsViewModels;
using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ANK19_ETicaretMVC.Areas.Seller.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Seller")]
    public class SellerReportController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var token = HttpContext.Session.GetString("JwtToken"); // Token'ı al
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token); // Token'ı çöz
            var sellerIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;//guid olan userId

            if (sellerIdClaim == null)
            {
                return Unauthorized("Seller ID bulunamadı.");
            }
            var sellerId = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerIdClaim}");//int olan sellerId
            ViewBag.SellerId = sellerId;
            var list = await _httpClient.GetFromJsonAsync<List<OrderProductReportViewModel>>($"Seller/SellerReport/GetOrderProductReports?sellerId={sellerId}");
            return View(list);

        }

        public async Task<IActionResult> MonthlyReport()
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
            ViewBag.SellerId = sellerId;
            var monthlylist = await _httpClient.GetFromJsonAsync<List<MonthlyOrderReportViewModel>>($"Seller/SellerReport/GetMonthlyOrders?sellerId={sellerId}&year={2025}");

            return View(monthlylist);

        }
    }
}
