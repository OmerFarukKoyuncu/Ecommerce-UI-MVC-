using ANK19_ETicaretMVC.Areas.Admin.Models.OrderReport;
using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]/{id?}")]
    [Area("Admin")]


   
    public class OrderReportController : Controller
    {
        public async Task<IActionResult> Orders()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<List<OrderReportViewModel>>("Admin/OrderReport/GetOrderReports"));
        }

        public async Task<IActionResult> OrderCustomers()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<List<OrderCustomerReportViewModel>>("Admin/OrderReport/GetOrderCustomerReports"));
        }
        public async Task<IActionResult> OrderSellers()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<List<OrderSellerReportViewModel>>("Admin/OrderReport/GetOrderSellerReports"));
        }

        public async Task<IActionResult> Index()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            return View(await _httpClient.GetFromJsonAsync<List<OrderProductReportViewModel>>("Admin/OrderReport/GetOrderProductReports"));
        }

        
    }
}
