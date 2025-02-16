using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels;
using ANK19_ETicaretMVC.ViewModels.ProductViewModels;
using ANK19_ETicaretMVC.ViewModels.SellerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Area ("Admin")]
    [Route ("[area]/[controller]/[action]/{id?}")]
    
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index ()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var values = await _httpClient.GetFromJsonAsync<List<ProductViewModel>>("Admin/Product/GetAllProducts");
            return View(values);
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var response = await _httpClient.GetFromJsonAsync<ProductDetailsViewModel>($"Admin/Product/GetProductDetails/{id}");
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var sellers = await _httpClient.GetFromJsonAsync<List<SellerViewModel>>("Admin/Seller/GetAllSeller");
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Admin/Category/GetAllCategories");
            ViewData["Categories"] = categories;
            ViewData["Sellers"] = sellers;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductViewModel model)
        {
            try
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                // MultipartFormDataContent oluştur
                var formData = new MultipartFormDataContent();

                // Basit türleri ekle
                formData.Add(new StringContent(model.Name), "Name");
                formData.Add(new StringContent(model.Description), "Description");
                formData.Add(new StringContent(model.Price.ToString()), "Price");
                formData.Add(new StringContent(model.Stock.ToString()), "Stock");
                formData.Add(new StringContent(model.SellerId.ToString()), "SellerId");

                // Kategorileri ekle
                if (model.CategoryIds != null)
                {
                    foreach (var categoryId in model.CategoryIds)
                    {
                        formData.Add(new StringContent(categoryId.ToString()), "CategoryIds");
                    }
                }
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
                var response = await _httpClient.PostAsync("Admin/Product/AddProduct", formData);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Admin/Category/GetAllCategories");
                    ViewData["Categories"] = categories;
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Admin/Category/GetAllCategories");
                ViewData["Categories"] = categories;
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(model);
            }
        }
        public async Task<IActionResult> Edit (int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            TempData["Id"] = id;
            var response = await _httpClient.GetFromJsonAsync<UpdateProductViewModel>($"Admin/Product/GetProductDetails/{id}");

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Edit (UpdateProductViewModel collection)
        {
            try
            {
                var formData = new MultipartFormDataContent();

                HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

                formData.Add(new StringContent(collection.Id.ToString()), "Id");
                formData.Add(new StringContent(collection.Name), "Name");
                formData.Add(new StringContent(collection.Description), "Description");
                formData.Add(new StringContent(collection.Price.ToString()), "Price");
                formData.Add(new StringContent(collection.Stock.ToString()), "Stock");
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + collection.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                collection.PhotoUrl = $"/uploads/{uniqueFileName}";
                formData.Add(new StringContent(collection.PhotoUrl), "PhotoUrl");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    collection.Photo.CopyTo(fileStream);
                }
                if (collection.Photo != null)
                {
                    var fileStreamContent = new StreamContent(collection.Photo.OpenReadStream());
                    formData.Add(fileStreamContent, "Photo", collection.Photo.FileName);
                }
             
                await  _httpClient.PutAsync($"Admin/Product/UpdateProduct", formData);
                return RedirectToAction (nameof (Index));
            }
            catch
            {
                return View ();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            var response = await _httpClient.DeleteAsync($"Admin/Product/DeleteProduct/{id}");

                return RedirectToAction(nameof(Index));
            
        }
    }
}
