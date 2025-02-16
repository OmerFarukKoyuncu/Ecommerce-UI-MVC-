using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]/[action]/{id?}")]

    public class CustomerProductDetailsController : Controller
    {
       

        public async Task<IActionResult> Index(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            var response = await _httpClient.GetFromJsonAsync<ProductDetailsViewModel>($"Customer/CustomerProductDetails/GetProductDetails/{id}");



            return View(response);
        }



    }
}
