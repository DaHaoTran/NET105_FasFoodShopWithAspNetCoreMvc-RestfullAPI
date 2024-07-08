using UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace UI.Controllers
{
    public class OrdersMngController : Controller
    {
        private readonly HttpClient _httpClient;
        public OrdersMngController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static int take = 10;
        private static int skip = 0;
        private static int total = 0;
        private static string savState = string.Empty;

        public async Task<IActionResult> Index(string state)
        {
            try
            {
                if(string.IsNullOrEmpty(state))
                {
                    state = savState;
                }
                var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/" + state);
                string apiresponse = await response.Content.ReadAsStringAsync();
                List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(apiresponse)!;
                if (total != orders.Count)
                {
                    total = orders.Count;
                }
                savState = state;
                return View(orders.Skip(skip).Take(take));
            }
            catch
            {
                TempData["Message"] = "Access with no data in database";
                return View(new List<Order>());
            }
        }

        public async Task<IActionResult> Search(string searchstr)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/string/" + searchstr);
            var apiresponse = await response.Content.ReadAsStringAsync();
            Order order = JsonConvert.DeserializeObject<Order>(apiresponse)!;
            if (order == default)
            {
                TempData["Message"] = "Not found";
                return View(new Order());
            }
            else
            {
                TempData["Message"] = "Founded";
                return RedirectToAction("Index", new {searchstr = savState});
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

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/int/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync()!;
            Order order = JsonConvert.DeserializeObject<Order>(apiresponse)!;
            var addrresponse = await _httpClient.GetAsync("https://localhost:7241/api/orders/address/" + order.CInforId);
            var addrapiresponse = await addrresponse.Content.ReadAsStringAsync()!;
            ViewBag.Address = JsonConvert.DeserializeObject<CustomerInformation>(addrapiresponse);
            var itemsresponse = await _httpClient.GetAsync("https://localhost:7241/api/orders/items/" + order.OrderId);
            var itemsapiresponse = await itemsresponse.Content.ReadAsStringAsync();
            ViewBag.Items = JsonConvert.DeserializeObject<List<OrderItem>>(itemsapiresponse);
            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/int/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync()!;
            Order order = JsonConvert.DeserializeObject<Order>(apiresponse)!;
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(order), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7241/api/orders/" + order.OrderId, content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse))
                {
                    TempData["Message"] = "Edit Order sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Edit Failed with not catching exception";
                    return RedirectToAction("Edit");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Edit Failed " + ex.InnerException?.Message;
                return RedirectToAction("Edit");
            }
        }
    }
}
