using ANK19_ETicaretMVC.Areas.Admin.Models.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ANK19_ETicaretMVC.Helpers;
using Newtonsoft.Json;

namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: UserController
        public async Task<ActionResult> Index()
        {
            // Hata mesajlarını TempData'dan alıyoruz
            if (TempData["Errors"] != null)
            {
                ViewBag.Errors = TempData["Errors"];


                TempData.Remove("Errors");
            }

            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];


                TempData.Remove("Success");
            }

            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var values = await _httpClient.GetFromJsonAsync<List<GetAllUsersWithRolesViewModel>>("Admin/User/GetAllUsersWithRoles");
            return View(values);
        }



        public async Task<ActionResult> Role(string userId)
        {
            TempData["id"] = userId;
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var user = await _httpClient.GetFromJsonAsync<UpdateUserViewModel>($"Admin/User/GetUserById?id={userId}");
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Role(string userId, bool admin, bool user, bool seller)
        {
            var queryString = $"?userId={userId}&admin={admin}&user={user}&seller={seller}";
            var url = "Admin/Role/UpdateUserRoles" + queryString;


            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var response = await _httpClient.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("ErrorPage");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddUserViewModel addUserViewModel)
        {

            if (!ModelState.IsValid)
            {
                // Hataları TempData'ya eklemek yerine ViewBag'e aktar
                TempData["Errors"] = ModelState.Values
                    .Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Eğer hata varsa Index sayfasına yönlendir
                return RedirectToAction("Index");
            }
            // Kullanıcıyı kaydetme işlemleri
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                // Fotoğraf varsa kaydetme işlemi
                if (addUserViewModel.Photo != null)
                {
                    var filePath = Path.Combine("wwwroot/images", addUserViewModel.Photo.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await addUserViewModel.Photo.CopyToAsync(stream);
                    }
                    addUserViewModel.ProfilePictureUrl = $"/images/{addUserViewModel.Photo.FileName}";
                }

                // Veriyi API'ye gönderme
                var message = await _httpClient.PostAsJsonAsync("Admin/User/AddUser", addUserViewModel);
                TempData["Success"] = "Kullanıcı Başarıyla Eklendi";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Errors"] = new[] { $"Bir hata oluştu: {ex.Message}" };
                return RedirectToAction("Index");
            }
        }



        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            TempData["id"] = id;
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var user = await _httpClient.GetFromJsonAsync<UpdateUserViewModel>($"Admin/User/GetUserById?id={id}");
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UpdateUserViewModel updateUserViewModel)
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
                return RedirectToAction(nameof(Index));
            }


            await _httpClient.PutAsJsonAsync($"Admin/User/UpdateUserById?id={id}", updateUserViewModel);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            var loginRequest = new LoginDto
            {
                Username = username,
                Password = password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8,
                "application/json");

            // API'ye POST isteği gönder
            var response = await _httpClient.PostAsync("Auth/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                if (tokenResponse != null)
                {
                    // Token'ı HttpContext.Items içine ekle
                    HttpContext.Session.SetString("JwtToken", tokenResponse.Token);
                    var cookieOptions = new CookieOptions
                    {
                        //HttpOnly = true, // XSS saldırılarına karşı koruma
                        //Secure = true, // HTTPS gerektirir
                        //SameSite = SameSiteMode.Strict, // Çerez paylaşımını kısıtla
                        Expires = DateTime.UtcNow.AddMinutes(3000) // Çerez süresi
                    };


                    Response.Cookies.Append("JwtToken", tokenResponse.Token, cookieOptions);


                    var role = JwtHelper.GetRoleFromJwt(tokenResponse.Token);

                    //if (role == "Admin")
                    //{
                    //    return Redirect("/Admin/Product/Index");
                    //}
                    //if (role == "Seller")
                    //{
                    //    return Redirect("/Seller/Product/Index");
                    //}
                    //if (role == "User")
                    //{
                    //    return Redirect("/");
                    //}

                    return Redirect("/Home/Index");

                }
            }

            // Başarısız login
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            Response.Cookies.Delete(".AspNetCore.Session");

            return Redirect("/");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
                await _httpClient.DeleteAsync($"Admin/User/DeleteUserById?id={id}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}



