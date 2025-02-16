using System.IdentityModel.Tokens.Jwt;
using ANK19_ETicaretMVC.Areas.Seller.Models.SellerProfileViewModels;
using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Route("[area]/[controller]/[action]")]
    public class SellerProfileController : Controller
    {
        // seller profile and profile update
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                // Eğer token yoksa kullanıcıyı login sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }

            // JWT token'ı decode et
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var sellerId = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={userId}");//int olan sellerId


            if (sellerId == null)
            {
                // Eğer id bulunamazsa, uygun bir hata işlemi yapabilirsiniz
                return RedirectToAction("Error", "Home");
            }

            // API isteği yapmak için HttpClient kullanın
          
            var seller = await _httpClient.GetFromJsonAsync<SellerProfileViewModel>($"Seller/SellerProfile/GetProfile");

            // Admin profil bilgilerini View'a gönder
            return View("Profile",seller);
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

            var seller = await _httpClient.GetFromJsonAsync<SellerProfileViewModel>($"Seller/SellerProfile/GetProfile");

            return View(seller);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(SellerProfileViewModel sellerProfileViewModel)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                // Eğer token yoksa kullanıcıyı login sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }

            // JWT token'ı decode et
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            sellerProfileViewModel.Id= await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={userId}");//int olan sellerId
            if (sellerProfileViewModel.ProfilePicture != null)
            {
                var filePath = Path.Combine("wwwroot/images", sellerProfileViewModel.ProfilePicture.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await sellerProfileViewModel.ProfilePicture.CopyToAsync(stream);
                }
                sellerProfileViewModel.ProfilePictureUrl = $"/images/{sellerProfileViewModel.ProfilePicture.FileName}";

                //var user2= await _httpClient.PutAsJsonAsync("Admin/User/UpdateUserById?id=" + id, updateUserViewModel);
                await _httpClient.PutAsJsonAsync($"Seller/SellerProfile/UpdateProfile", sellerProfileViewModel);
                return RedirectToAction("Profile", "SellerProfile");
            }

            //await _httpClient.PutAsJsonAsync("Admin/User/UpdateUserById?id=" + id, updateUserViewModel);
            await _httpClient.PutAsJsonAsync($"Seller/SellerProfile/UpdateProfile", sellerProfileViewModel);
            return RedirectToAction("Profile", "SellerProfile");


        }

    }
}
