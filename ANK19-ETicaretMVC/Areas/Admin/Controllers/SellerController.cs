using ANK19_ETicaretMVC.Helpers;
using ANK19_ETicaretMVC.ViewModels;
using ANK19_ETicaretMVC.ViewModels.SellerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ANK19_ETicaretMVC.Areas.Admin.Controllers
{
    [Area ("Admin")]
    [Route ("[area]/[controller]/[action]/{id?}")]
    public class SellerController : Controller
    {

        
        // GET: SellerController
        public async Task<IActionResult> Index ()
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var values = await _httpClient.GetFromJsonAsync<List<SellerViewModel>> ("Admin/Seller/GetAllSeller");
            return View (values);
        }

        // GET: SellerController/Details/5

        public async Task<IActionResult> Details (int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var value = await _httpClient.GetFromJsonAsync<SellerViewModel> ($"Admin/Seller/GetSellerById/{id}");
            return View (value);
        }


        public async Task<IActionResult> Aproved(int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var response = await _httpClient.PostAsync ($"Admin/Seller/GetApprovedSeller/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync ();
                return RedirectToAction ("Index");
            }
            else
            {
                return BadRequest ($"API hatası: {response.StatusCode}, {await response.Content.ReadAsStringAsync ()}");
            }

        }

        // GET: SellerController/Create
        public ActionResult Create ()
        {
           
            return View ();
        }

        // POST: SellerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (SellerViewModel sellerViewModel)
        {
            try
            {

                if (sellerViewModel != null)
                {
                    HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
                    await _httpClient.PostAsJsonAsync ("Admin/Seller/AddSeller",sellerViewModel);
                }
                return RedirectToAction (nameof (Index));
            }
            catch
            {
                return View ();
            }
        }

        // GET: SellerController/Edit/5
        public async Task<IActionResult> Edit (int id)
        {
            HttpClient _httpClient = HttpClientInstance.CreateClient(HttpContext);
            var value = await _httpClient.GetFromJsonAsync<SellerViewModel> ($"Admin/Seller/GetSellerById/{id}");
            return View (value);
        }

        // POST: SellerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (SellerViewModel sellerViewModel)
        {
            try
            {

                return RedirectToAction (nameof (Index));
            }
            catch
            {
                return View ();
            }
        }

        // GET: SellerController/Delete/5
        public ActionResult Delete (int id)
        {
            return View ();
        }

        // POST: SellerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete (int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction (nameof (Index));
            }
            catch
            {
                return View ();
            }
        }
    }
}
