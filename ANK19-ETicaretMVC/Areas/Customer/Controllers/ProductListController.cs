using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels.CustomerProductViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ANK19_ETicaretMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]/[action]")]
    public class ProductListController : Controller
    {
        //// GET: ProductListController
        //public async Task<IActionResult> Index()
        //{
        //    HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
        //    var products = await _httpClient.GetFromJsonAsync<List<CustomerProductViewModel>>("Customer/CustomerProductList/GetProductList");

        //    return View(products);
        //}


        public async Task<IActionResult> Index(string category)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            List<CustomerProductViewModel> products;

            if (!string.IsNullOrEmpty(category))
            {
                // Eğer kategori adı varsa, ilgili API çağrısını yap
                products = await _httpClient.GetFromJsonAsync<List<CustomerProductViewModel>>(
                    $"Customer/CustomerProductList/GetProductListByCategoryName?name={category}");
            }
            else
            {
                // Eğer kategori adı yoksa, tüm ürünleri getir
                products = await _httpClient.GetFromJsonAsync<List<CustomerProductViewModel>>(
                    "Customer/CustomerProductList/GetProductList");
            }

            return View(products);
        }


        public async Task<IActionResult> GetCategories()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("Customer/CustomerProductList/GetProductCategoryList");
            return Json(categories);
        }


        // GET: ProductListController/Details/5
        public async Task<IActionResult> Details(string name)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);

            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index"); // Eğer arama boşsa, anasayfaya yönlendir
            }

            var products = await _httpClient.GetFromJsonAsync<List<CustomerProductViewModel>>($"Customer/CustomerProductList/GetProductListByName?name={Uri.EscapeDataString(name)}");

            return View("Index", products);
        }

        // GET: ProductListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
