using Microsoft.AspNetCore.Mvc;
using ANK19_ETicaretMVC.Areas.Admin.Data;
using ANK19_ETicaretMVC.Areas.Seller.Models.PromotionViewModels;
using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.ProductViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace ANK19_ETicaretMVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class PromotionController : Controller
    {
        private readonly SchafoldIcinDbContext _context;

        public PromotionController(SchafoldIcinDbContext context)
        {
            _context = context;
        }

        // GET: Seller/Promotion
        public async Task<IActionResult> Index()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Token'dan kullanıcı ID'sini al (genellikle "sub" veya özel bir claim'den gelir)
            var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value.ToString();
            var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId
            var values = await _httpClient.GetFromJsonAsync<List<PromotionViewModel>>($"Seller/Promotion/GetPromotions");
            return View(values);
        }

        // GET: Seller/Promotion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            TempData["Id"] = id;

            // API'den promosyon detaylarını getir
            var response = await _httpClient.GetFromJsonAsync<PromotionViewModel>($"Seller/Promotion/GetPromotion/{id}");

            return View(response);
        }

        // GET: Seller/Promotion/Create
        public async Task<IActionResult> Create()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            // Kullanıcı kimliğini JWT token'dan çek
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Token'dan kullanıcı ID'sini al (genellikle "sub" veya özel bir claim'den gelir)
            var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value.ToString();
            var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId
            var products = await _httpClient.GetFromJsonAsync<List<ProductViewModel>>($"Seller/Product/GetProductsBySeller?sellerId={sellerIdInt}");

            // Kategorileri ViewData'ya ekle
            ViewData["Products"] = products;
            return View();
        }

        // POST: Seller/Promotion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AddPromotionViewModel promotionViewModel)
        {
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                // Kullanıcı kimliğini JWT token'dan çek
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Token'dan kullanıcı ID'sini al (genellikle "sub" veya özel bir claim'den gelir)
                var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value.ToString();
                var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId

                if (string.IsNullOrEmpty(sellerId))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı kimliği doğrulanamadı.");
                    return View(promotionViewModel);
                }

                // MultipartFormDataContent oluştur
                var formData = new MultipartFormDataContent();

                // Basit türleri ekle
                formData.Add(new StringContent(promotionViewModel.ProductId.ToString()), "ProductId");
                formData.Add(new StringContent(promotionViewModel.PromotionName), "PromotionName");
                formData.Add(new StringContent(promotionViewModel.DiscountValue.ToString()), "DiscountValue");
                formData.Add(new StringContent(promotionViewModel.StartDate.ToString()), "StartDate");
                formData.Add(new StringContent(promotionViewModel.EndDate.ToString()), "EndDate");
                formData.Add(new StringContent(promotionViewModel.DiscountType.ToString()), "DiscountType");
              
                
                 

                // API'ye POST isteği gönder
                var response = await _httpClient.PostAsync("Seller/Promotion/PostPromotion", formData);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, "Promosyon eklenirken bir hata oluştu.");
                    return View(promotionViewModel);
                }
            }
            catch (Exception ex)
            {
                
                
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(promotionViewModel);
            }
        }

        // GET: Seller/Promotion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {


            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            // Kullanıcı kimliğini JWT token'dan çek
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Token'dan kullanıcı ID'sini al (genellikle "sub" veya özel bir claim'den gelir)
            var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value.ToString();
            var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId
            var products = await _httpClient.GetFromJsonAsync<List<ProductViewModel>>($"Seller/Product/GetProductsBySeller?sellerId={sellerIdInt}");

            // Kategorileri ViewData'ya ekle
            ViewData["Products"] = products;
         
             

            TempData["Id"] = id;

            // API'den promosyon detaylarını getir
            var response = await _httpClient.GetFromJsonAsync<PromotionViewModel>($"Seller/Promotion/GetPromotion/{id}");

            return View(response);
        }

        // POST: Seller/Promotion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddPromotionViewModel promotionViewModel)
        {
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                // Kullanıcı kimliğini JWT token'dan çek
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Token'dan kullanıcı ID'sini al (genellikle "sub" veya özel bir claim'den gelir)
                var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value.ToString();
                var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId

                if (string.IsNullOrEmpty(sellerId))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı kimliği doğrulanamadı.");
                    return View(promotionViewModel);
                }

                // MultipartFormDataContent oluştur
                var formData = new MultipartFormDataContent();

                // Basit türleri ekle 
                formData.Add(new StringContent(promotionViewModel.ProductId.ToString()), "ProductId");
                formData.Add(new StringContent(promotionViewModel.PromotionName), "PromotionName");
                formData.Add(new StringContent(promotionViewModel.DiscountValue.ToString()), "DiscountValue");
                formData.Add(new StringContent(promotionViewModel.StartDate.ToString()), "StartDate");
                formData.Add(new StringContent(promotionViewModel.EndDate.ToString()), "EndDate");
                formData.Add(new StringContent(promotionViewModel.DiscountType.ToString()), "DiscountType");




                // API'ye POST isteği gönder
                var response = await _httpClient.PutAsync($"Seller/Promotion/PutPromotion/{id}", formData);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Promosyon güncellenirken bir hata oluştu.");
                    return View(promotionViewModel);
                }
            }
            catch (Exception ex)
            {


                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(promotionViewModel);
            }
        }

        // GET: Seller/Promotion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            TempData["Id"] = id;

            // API'den promosyon detaylarını getir
            var response = await _httpClient.GetFromJsonAsync<PromotionViewModel>($"Seller/Promotion/GetPromotion/{id}");

            return View(response);
        }

        // POST: Seller/Promotion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            PromotionViewModel response = new PromotionViewModel();
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                // Kullanıcı kimliğini JWT token'dan çek
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Token'dan kullanıcı ID'sini al (genellikle "sub" veya özel bir claim'den gelir)
                var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value.ToString();
                var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId

                  _httpClient = HttpClientInstance.CreateClient(HttpContext);

                TempData["Id"] = id;

                // API'den promosyon detaylarını getir
                  response = await _httpClient.GetFromJsonAsync<PromotionViewModel>($"Seller/Promotion/GetPromotion/{id}");
 


                if (string.IsNullOrEmpty(sellerId))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı kimliği doğrulanamadı.");
                    return View(response);
                }

                 
                var  response2 = await _httpClient.DeleteAsync($"Seller/Promotion/DeletePromotion/{id}" );
                var content = await response2.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (response2.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Promosyon güncellenirken bir hata oluştu.");
                    return View(response);
                }
            }
            catch (Exception ex)
            {


                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(response);
            }
        }

        
    }
}
