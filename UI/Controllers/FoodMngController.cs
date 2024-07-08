using UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace UI.Controllers
{
    public class FoodMngController : Controller
    {
        private readonly HttpClient _httpClient;
        public FoodMngController(HttpClient httpClient)
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
                var response = await _httpClient.GetAsync("https://localhost:7241/api/foods");
                string apiresponse = await response.Content.ReadAsStringAsync();
                List<Food> foods = JsonConvert.DeserializeObject<List<Food>>(apiresponse)!;
                if (total != foods.Count)
                {
                    total = foods.Count;
                }
                return View(foods.Skip(skip).Take(take));
            }
            catch
            {
                TempData["Message"] = "Access with no data in database";
                return View(new List<Food>());
            }
        }

        public async Task<IActionResult> Search(string searchstr)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/foods/string/" + searchstr);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Food food = JsonConvert.DeserializeObject<Food>(apiresponse)!;
            if (food == default)
            {
                TempData["Message"] = "Not found";
                return View(new Food());
            }
            else
            {
                TempData["Message"] = "Founded";
                return View(food);
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

        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/foods/foodtype/name");
            var apiresponse = await response.Content.ReadAsStringAsync()!;
            ViewBag.FTypeName = JsonConvert.DeserializeObject<List<string>>(apiresponse)!;
            var response2 = await _httpClient.GetAsync("https://localhost:7241/api/foods/foodcategory/name");
            var apiresponse2 = await response2.Content.ReadAsStringAsync()!;
            ViewBag.FCategoryName = JsonConvert.DeserializeObject<List<string>>(apiresponse2)!;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Food food)
        {
            try
            {
                food.AdminCode = HttpContext.Session.GetString("AdminEmail");
                StringContent content = new StringContent(JsonConvert.SerializeObject(food), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7241/api/foods", content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Add new Food sucessfully";
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
            var response = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Food food = JsonConvert.DeserializeObject<Food>(apiresponse)!;
            var response1 = await _httpClient.GetAsync("https://localhost:7241/api/foods/foodtype/name");
            var apiresponse1 = await response1.Content.ReadAsStringAsync()!;
            ViewBag.FTypeName = JsonConvert.DeserializeObject<List<string>>(apiresponse1)!;
            var response2 = await _httpClient.GetAsync("https://localhost:7241/api/foods/foodcategory/name");
            var apiresponse2 = await response2.Content.ReadAsStringAsync()!;
            ViewBag.FCategoryName = JsonConvert.DeserializeObject<List<string>>(apiresponse2)!;
            return View(food);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Food food)
        {
            try
            {
                food.AdminCode = HttpContext.Session.GetString("AdminEmail");
                StringContent content = new StringContent(JsonConvert.SerializeObject(food), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7241/api/foods/" + food.FoodCode, content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Update Food sucessfully";
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
            var response = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Food food = JsonConvert.DeserializeObject<Food>(apiresponse)!;
            return View(food);
        }

        [HttpPost]
        public async Task<IActionResult> delete(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7241/api/foods/" + id);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Delete Food sucessfully";
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
            var response = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Food food = JsonConvert.DeserializeObject<Food>(apiresponse)!;
            return View(food);
        }
    }
}
