using UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UI.Controllers
{
    public class FoodCategoryMngController : Controller
    {
        private readonly HttpClient _httpClient;
        public FoodCategoryMngController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static int take = 10;
        private static int skip = 0;
        private static int total = 0;

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7241/api/categories");
                string apiresponse = await response.Content.ReadAsStringAsync();
                List<FoodCategory> categories = JsonConvert.DeserializeObject<List<FoodCategory>>(apiresponse)!;
                if (total != categories.Count)
                {
                    total = categories.Count;
                }
                return View(categories.Skip(skip).Take(take));
            }
            catch
            {
                TempData["Message"] = "Access with no data in database";
                return View(new List<FoodCategory>());
            }
        }

        public async Task<IActionResult> Search(string searchstr)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/categories/string/" + searchstr);
            var apiresponse = await response.Content.ReadAsStringAsync();
            FoodCategory fcate = JsonConvert.DeserializeObject<FoodCategory>(apiresponse)!;
            if (fcate == default)
            {
                TempData["Message"] = "Not found";
                return View(new FoodCategory());
            }
            else
            {
                TempData["Message"] = "Founded";
                return View(fcate);
            }
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Next()
        {
            if (skip + 10 >= total)
            {
                TempData["Message"] = "Reached maximum value";
            }
            else
            {
                skip += 10;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Prev()
        {
            if (skip - 10 < 0)
            {
                TempData["Message"] = "Reached minimum value";
            }
            else
            {
                skip -= 10;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodCategory fcate)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(fcate), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7241/api/categories", content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Add new Food category sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Add Failed with not catching exception";
                    return RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Add Failed " + ex.InnerException?.Message;
                return RedirectToAction("Create");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/categories/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            FoodCategory fcate = JsonConvert.DeserializeObject<FoodCategory>(apiresponse)!;
            return View(fcate);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodCategory fcate)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(fcate), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7241/api/categories/" + fcate.FCategoryCode, content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Update Food category sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Update Failed with not catching exception";
                    return RedirectToAction("Edit");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Update Failed " + ex.InnerException?.Message;
                return RedirectToAction("Edit");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/categories/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            FoodCategory fcate = JsonConvert.DeserializeObject<FoodCategory>(apiresponse)!;
            return View(fcate);
        }

        [HttpPost]
        public async Task<IActionResult> delete(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7241/api/categories/" + id);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Delete Food category sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Delete Failed Because It is existing foreign keys";
                    return RedirectToAction("Delete");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Delete Failed " + ex.InnerException?.Message;
                return RedirectToAction("Delete");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/categories/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            FoodCategory fcate = JsonConvert.DeserializeObject<FoodCategory>(apiresponse)!;
            return View(fcate);
        }
    }
}
