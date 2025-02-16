using ANK19_ETicaretMVC.Areas.Seller.Models.SellerProfileViewModels;
using ANK19_ETicaretMVC.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using ANK19_ETicaretMVC.ViewModels.ProductViewModels;
using ANK19_ETicaretMVC.Areas.Customer.Models.CustomerProfileViewModels;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]/[action]")]
    public class CustomerProfileController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var customer = await _httpClient.GetFromJsonAsync<CustomerViewModel>("Customer/CustomerProfile/GetProfile");
            return View("Profile",customer);

        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            // JWT token'ını kontrol et
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var customer = await _httpClient.GetFromJsonAsync<CustomerViewModel>($"Customer/CustomerProfile/GetProfile");

            return View(customer);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromForm]CustomerViewModel customerViewModel)
        {


            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var token = HttpContext.Session.GetString("JwtToken");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            customerViewModel.Id = userId;

            if (customerViewModel.ProfilePicture != null)
            {
                var filePath = Path.Combine("wwwroot/images", customerViewModel.ProfilePicture.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await customerViewModel.ProfilePicture.CopyToAsync(stream);
                }
                customerViewModel.ProfilePictureUrl = $"/images/{customerViewModel.ProfilePicture.FileName}";

                //var user2= await _httpClient.PutAsJsonAsync("Admin/User/UpdateUserById?id=" + id, updateUserViewModel);
                await _httpClient.PutAsJsonAsync($"Customer/CustomerProfile/UpdateProfile", customerViewModel);
                return RedirectToAction("Profile", "CustomerProfile");
            }
            else
            {
                // Yeni resim yüklenmediğinde eski resmi koru
                var existingProfile = await _httpClient.GetFromJsonAsync<CustomerViewModel>(
                    $"Customer/CustomerProfile/GetProfile");

                customerViewModel.ProfilePictureUrl = existingProfile.ProfilePictureUrl;
            }
                //await _httpClient.PutAsJsonAsync($"Customer/CustomerProfile/UpdateProfile", customerViewModel);
            return RedirectToAction("Profile", "CustomerProfile");
        }
    }
}
