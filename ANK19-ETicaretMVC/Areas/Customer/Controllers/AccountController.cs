using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.CustomerSellerRegisterViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Route ("[area]/[controller]/[action]/{id?}")]
    [Area ("Customer")]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public async Task<IActionResult> CustomerRegister ()
        {

            return View ();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CustomerRegister (CustomerRegisterViewModel model)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient (HttpContext);

            if (!ModelState.IsValid) { return View (); }

            if (model.Photo != null)
            {
                var filePath = Path.Combine ("wwwroot/images/", model.Photo.FileName);

                using (var stream = new FileStream (filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync (stream);
                }
                model.ProfilePictureUrl = $"/images/{model.Photo.FileName}";
            }


            var response = await _httpClient.PostAsJsonAsync("Customer/AccountRegister/CustomerRegister", model);

            if (response.IsSuccessStatusCode)
            {
            return RedirectToAction ("Index", "Home", new { area = "Home" });
            }
            return View ();
        }

        [AllowAnonymous]
        public async Task<IActionResult> StoreRegister ()
        {
            return View ();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> StoreRegister (SellerRegisterViewModel model)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient (HttpContext);

            var token = HttpContext.Session.GetString ("JwtToken");

            if (string.IsNullOrEmpty (token))
            {
                return RedirectToAction ("Index", "Home", new { area = "Home" });
            }

            var handler = new JwtSecurityTokenHandler ();
            var jwtToken = handler.ReadJwtToken (token);
            var userId = jwtToken.Claims.FirstOrDefault (c => c.Type == "sub")?.Value;

            if (jwtToken.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Role)?.Value.ToString () == "Seller")
            {
                return RedirectToAction (Url.Action ("Index", "Product", new { area = "Seller" }));
            }

            if (!ModelState.IsValid) { return View (); }

            model.UserId = userId;

            if (model.Photo != null)
            {
                var filePath = Path.Combine ("wwwroot/images/", model.Photo.FileName);

                using (var stream = new FileStream (filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync (stream);
                }
                model.ProfilePictureUrl = $"/images/{model.Photo.FileName}";
            }


            var respone = await _httpClient.PostAsJsonAsync("Customer/AccountRegister/SellerRegister", model);



            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction ("Logout", "User", new { area = "Admin" });
            }
            return View ();
            

        }
    }
}
