using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using ANK19_ETicaretMVC.Areas.Customer.Models.CustomerOrderViewModels;
using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]/[action]")]
    public class CustomerOrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
                var orders = await _httpClient.GetFromJsonAsync<List<CustomerOrderViewModel>>("Customer/CustomerOrder/GetOrders");

                if (orders == null || !orders.Any())
                {
                    TempData["ErrorMessage"] = "Henüz siparişiniz bulunmamaktadır.";
                    return View(new List<CustomerOrderViewModel>());
                }

                return View(orders);
            }
            catch(Exception ex)
            {
                throw;
            }
           
        }
    }
}
