using UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Session;

namespace UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient; 
        }

        [Route("/Adminaccess")]
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("AdminEmail") == null)
            {
                LogState.IsLoged = false;
                LogState.Email = string.Empty;
            }
            try
            {
                var orderresponse = await _httpClient.GetAsync("https://localhost:7241/api/orders/" + "Delivered");
                string orderapiresponse = await orderresponse.Content.ReadAsStringAsync();
                List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(orderapiresponse)!;
                ViewBag.Orderday= orders.Where(x => DateTime.Parse(x.DeliveryDate.ToString()!).Day == DateTime.Now.Day).Count();
                ViewBag.Ordermonth = orders.Where(x => DateTime.Parse(x.DeliveryDate.ToString()!).Month == DateTime.Now.Month).Count();
                ViewBag.Orderyear = orders.Where(x => DateTime.Parse(x.DeliveryDate.ToString()!).Year == DateTime.Now.Year).Count();
                ViewBag.Order = orders;
                var response = await _httpClient.GetAsync("https://localhost:7241/api/foods");
                string apiresponse = await response.Content.ReadAsStringAsync();
                List<Food> foods = JsonConvert.DeserializeObject<List<Food>>(apiresponse)!;
                return View(foods);
            }
            catch
            {
                TempData["Message"] = "Access with no data in database";
                return View(new List<Food>());
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
           if(ModelState.IsValid)
           {
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7241/api/login/admins", content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse) == false)
                {
                    TempData["Message"] = "Wrong email or password !";
                    return RedirectToAction("Login");
                }
                else
                {
                    HttpContext.Session.SetString("AdminEmail", admin.Email);
                    LogState.IsLoged = true;
                    LogState.Email = HttpContext.Session.GetString("AdminEmail")!;
                    var response1 = await _httpClient.GetAsync("https://localhost:7241/api/admins/string/" + LogState.Email);
                    var apiresponse1 = await response1.Content.ReadAsStringAsync();
                    Admin ad = JsonConvert.DeserializeObject<Admin>(apiresponse1)!;
                    LogState.Level = ad.Level;
                    return RedirectToAction("Index");
                }
           } 
           else
           {
                TempData["Message"] = "Email or password is invalid";
                return RedirectToAction("Login");
           }
        }

        public async Task<IActionResult> Logout()
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/login/admins/" + HttpContext.Session.GetString("AdminEmail"));
            LogState.IsLoged = false;
            LogState.Email = string.Empty;
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Index");
        }
    }
}
