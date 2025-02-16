using ANK19_ETicaretMVC.Areas.Admin.Models.AdminViewModels;
using ANK19_ETicaretMVC.Areas.Admin.Models.UserViewModels;
using ANK19_ETicaretMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;


namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")] 
    public class AdminController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
             
            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {          
                return RedirectToAction("Login", "Account");
            }       
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);           
            var id= jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (id == null)
            {
                
                return RedirectToAction("Error", "Home");
            }       
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var admin = await _httpClient.GetFromJsonAsync<AdminProfileViewModel>($"Admin/User/GetUserById?id={id}");       
            return View(admin);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string id, UpdateUserViewModel updateUserViewModel)
        {

            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            if (updateUserViewModel.Photo != null)
            {
                var filePath = Path.Combine("wwwroot/images", updateUserViewModel.Photo.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateUserViewModel.Photo.CopyToAsync(stream);
                }
                updateUserViewModel.ProfilePictureUrl = $"/images/{updateUserViewModel.Photo.FileName}";

                
                await _httpClient.PutAsJsonAsync($"Admin/User/UpdateUserById?id={id}", updateUserViewModel);
                return RedirectToAction("Index","User");
            }

            
            await _httpClient.PutAsJsonAsync($"Admin/User/UpdateUserById?id={id}", updateUserViewModel);
            return RedirectToAction("Index", "User");

        }
    }
}
