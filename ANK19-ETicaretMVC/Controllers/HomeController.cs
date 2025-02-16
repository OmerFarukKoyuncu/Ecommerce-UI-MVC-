using ANK19_ETicaretMVC.Areas.Seller.Models.PromotionViewModels;
using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.Models;
using ANK19_ETicaretMVC.ViewModels.ContentViewModels;
using ANK19_ETicaretMVC.ViewModels.CustomerProductViewModels;
using ANK19_ETicaretMVC.ViewModels.HomeViewModel;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ANK19_ETicaretMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController (ILogger<HomeController> logger)
        {
            _logger = logger;

            _httpClient = HttpClientInstance.CreateClient(HttpContext);

          

        }

        // GET: /Content/GetContent/ByName
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string? error)
        {
            if (error != null)
            {
                TempData["ErrorMessage"] = error;
            }

            var token = HttpContext.Session.GetString ("JwtToken");
            var handler = new JwtSecurityTokenHandler ();
            if (token != null)
            {
                var jwtToken = handler.ReadJwtToken (token);
                ViewBag.UserRole = jwtToken.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Role)?.Value.ToString ();
            }


            try
            {

                var products = await _httpClient.GetFromJsonAsync<List<CustomerProductViewModel>>("Customer/CustomerProductList/GetProductList");


                var content = await _httpClient.GetFromJsonAsync<ContentViewModel>("Admin/Content/GetContentByName?pageName=Home");

                var model = new HomeViewModel
                {
                    Products = products,
                    Content = content
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching Home content: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Content/GetContent/ByName
        [HttpGet]
        public async Task<IActionResult> AboutUs()
        {
            return View(await _httpClient.GetFromJsonAsync<ContentViewModel>(
                     "Admin/Content/GetContentByName?pageName=AboutUs"))
                 ;
        }

        // GET: /Content/GetContent/ByName
        [HttpGet]
        public async Task<IActionResult> ContactUs()
        {
            return View(await _httpClient.GetFromJsonAsync<ContentViewModel>(
                     "Admin/Content/GetContentByName?pageName=ContactUs"));
        }

        public IActionResult Privacy ()
        {
            return View ();
        }

       
        public IActionResult Error ()
        {
            return View ();
        }
    }
}
