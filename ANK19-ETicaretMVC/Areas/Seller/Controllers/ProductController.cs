using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.ProductViewModels;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class ProductController : Controller
    {


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
            var values = await _httpClient.GetFromJsonAsync<List<ProductViewModel>>($"Seller/Product/GetProductsBySeller?sellerId={sellerIdInt}");
            return View(values);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Admin/Category/GetAllCategories");
            ViewData["Categories"] = categories;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        public async Task<IActionResult> Create(AddProductViewModel model)
        {
            try
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

                if (string.IsNullOrEmpty(sellerId))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı kimliği doğrulanamadı.");
                    return View(model);
                }

                var formData = new MultipartFormDataContent();

                // Basit türleri ekle
                formData.Add(new StringContent(model.Name), "Name");
                formData.Add(new StringContent(model.Description), "Description");
                formData.Add(new StringContent(model.Price.ToString()), "Price");
                formData.Add(new StringContent(model.Stock.ToString()), "Stock");

                // SellerId'yi token'dan aldığımız ID olarak ekle
                //çevirilmiş ıd yi ekle 
                formData.Add(new StringContent(sellerIdInt.ToString()), "SellerId");

                // Kategorileri ekle
                if (model.CategoryIds != null)
                {
                    foreach (var categoryId in model.CategoryIds)
                    {
                        formData.Add(new StringContent(categoryId.ToString()), "CategoryIds");
                    }
                }

                // Dosya yükleme işlemi
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                model.PhotoUrl = $"/uploads/{uniqueFileName}";
                formData.Add(new StringContent(model.PhotoUrl), "PhotoUrl");

                // Dosyayı ekle
                if (model.Photo != null)
                {
                    var fileStreamContent = new StreamContent(model.Photo.OpenReadStream());
                    formData.Add(fileStreamContent, "Photo", model.Photo.FileName);
                }

                // API'ye POST isteği gönder
                var response = await _httpClient.PostAsync("Seller/Product/AddProduct", formData);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Admin/Category/GetAllCategories");
                    ViewData["Categories"] = categories;
                    ModelState.AddModelError(string.Empty, "Ürün eklenirken bir hata oluştu.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Admin/Category/GetAllCategories");
                ViewData["Categories"] = categories;
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(model);
            }
        }

        // GET: ProductController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            TempData["Id"] = id;

            // API'den ürün detaylarını getir
            var response = await _httpClient.GetFromJsonAsync<UpdateProductViewModel>($"Seller/Product/GetProductDetails/{id}");

            return View(response);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductViewModel collection)
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

                // Token'dan kullanıcı ID'sini al (örneğin "sub" veya özel bir claim'den gelebilir)
                var sellerId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var sellerIdInt = await _httpClient.GetFromJsonAsync<int>($"SellerOrder/GetSellerIdByUserId?sellerUserId={sellerId}");//int olan sellerId

                if (string.IsNullOrEmpty(sellerId))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı kimliği doğrulanamadı.");
                    return View(collection);
                }

                // MultipartFormDataContent oluştur
                var formData = new MultipartFormDataContent();

                // Basit türleri ekle
                formData.Add(new StringContent(collection.Id.ToString()), "Id");
                formData.Add(new StringContent(collection.Name), "Name");
                formData.Add(new StringContent(collection.Description), "Description");
                formData.Add(new StringContent(collection.Price.ToString()), "Price");
                formData.Add(new StringContent(collection.Stock.ToString()), "Stock");

                // Satıcı ID'yi token'dan çekerek ekle
                
                formData.Add(new StringContent(sellerIdInt.ToString()), "SellerId");

                // Fotoğraf işlemi
                if (collection.Photo != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + collection.Photo.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await collection.Photo.CopyToAsync(fileStream);
                    }

                    collection.PhotoUrl = $"/uploads/{uniqueFileName}";
                    formData.Add(new StringContent(collection.PhotoUrl), "PhotoUrl");

                    // Fotoğrafı form-data'ya ekle
                    var fileStreamContent = new StreamContent(collection.Photo.OpenReadStream());
                    formData.Add(fileStreamContent, "Photo", collection.Photo.FileName);
                }
                else
                {
                    formData.Add(new StringContent(collection.PhotoUrl ?? ""), "PhotoUrl");
                }

                // API'ye PUT isteği gönder
                var response = await _httpClient.PutAsync($"Seller/Product/UpdateProduct", formData);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ürün güncellenirken bir hata oluştu.");
                    return View(collection);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(collection);
            }
        }
        
      
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            var response = await _httpClient.GetFromJsonAsync<ProductDetailsViewModel>($"Seller/Product/GetProductDetails/{id}");



            return View(response);
        }


    }
}
