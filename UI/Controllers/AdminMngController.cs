
using UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Unicode;

namespace UI.Controllers
{
    public class AdminMngController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminMngController(HttpClient httpClient)
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
                var response = await _httpClient.GetAsync("https://localhost:7241/api/admins");
                string apiresponse = await response.Content.ReadAsStringAsync();
                List<Admin> admins = JsonConvert.DeserializeObject<List<Admin>>(apiresponse)!;
                if (total != admins.Count)
                {
                    total = admins.Count;
                }
                return View(admins.Skip(skip).Take(take));
            }
            catch
            {
                TempData["Message"] = "Access with no data in database";
                return View(new List<Admin>());
            }
        }

        public async Task<IActionResult> ForceOff(string id)
        {
            try
            {
                Admin admin = new Admin();
                admin.AdminCode = id;
                admin.IsOnl = false;
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7241/api/admins/" + admin.AdminCode, content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = $"Force Offline {id} admin sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = $"An error ocurred (Admin Code: {id})!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed" + ex.InnerException?.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Search(string searchstr)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/admins/string/" + searchstr);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Admin admin = JsonConvert.DeserializeObject<Admin>(apiresponse)!;
            if(admin == default)
            {
                TempData["Message"] = "Not found";
                return View(new Admin());
            }
            else
            {
                TempData["Message"] = "Founded";
                return View(admin);
            }
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Next()
        {
            if(skip + 10 >= total)
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
            if(skip - 10 < 0)
            {
                TempData["Message"] = "Reached minimum value";
            } else
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
        public async Task<IActionResult> Create(Admin admin)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7241/api/admins", content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Add new admin sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Add Failed with Same email or Inputs Error";
                    return RedirectToAction("Create");
                }
            } catch (Exception ex) 
            {
                TempData["Error"] = "Add Failed " + ex.InnerException?.Message;
                return RedirectToAction("Create");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/admins/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Admin admin = JsonConvert.DeserializeObject<Admin>(apiresponse)!;
            return View(admin);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/admins/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Admin admin = JsonConvert.DeserializeObject<Admin>(apiresponse)!;
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Admin admin)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7241/api/admins/" + admin.AdminCode, content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Update admin sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Update Failed Because account is being used";
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
            var response = await _httpClient.GetAsync("https://localhost:7241/api/admins/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Admin admin = JsonConvert.DeserializeObject<Admin>(apiresponse)!;
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> delete(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7241/api/admins/" + id);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Delete admin sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Delete Failed Because It is existing foreign keys Or Account is being used";
                    return RedirectToAction("Delete");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Delete Failed " + ex.InnerException?.Message;
                return RedirectToAction("Delete");
            }
        }
    }
}
