using UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using Azure;
using UI.Helperscustom;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using UI.Areas.Identity.Data;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        private async Task<List<Food>> GetFoods()
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/foods");
            var apirespone = await response.Content.ReadAsStringAsync();
            List<Food> foods = JsonConvert.DeserializeObject<List<Food>>(apirespone)!;
            return foods;
        }

        private async Task<List<FoodCategory>> GetCategories()
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/categories");
            var apiresponse = await response.Content.ReadAsStringAsync();
            List<FoodCategory> categories = JsonConvert.DeserializeObject<List<FoodCategory>>(apiresponse)!;
            return categories;
        }

        public async Task CreateNewCustomers()
        {
            LogStateC.NeedCreate = false;
            Customer customer = new Customer();
            customer.Email = LogStateC.Email;
            customer.PassWord = "Password Hash 123";
            customer.ConfirmPassWord = "Password Hash 123";
            customer.Email = customer.Email.Trim();
            customer.PassWord = customer.PassWord.Trim();
            customer.ConfirmPassWord = customer.ConfirmPassWord.Trim();
            StringContent content = new StringContent(JsonConvert.SerializeObject(customer), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7241/api/customers", content);
            var apiresponse = await response.Content.ReadAsStringAsync();
            if (JsonConvert.DeserializeObject<bool>(apiresponse) == false)
            {
                var response3 = await _httpClient.PutAsync("https://localhost:7241/api/login/customers", content);
                var apiresponse3 = await response3.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse3))
                {
                    HttpContext.Session.SetString("CustomerEmail", customer.Email);
                    LogStateC.IsLoggedIn = true;
                    LogStateC.Email = HttpContext.Session.GetString("CustomerEmail")!;
                    LogStateC.Username = CutStringBeforeAt(LogStateC.Email);
                    var response2 = await _httpClient.GetAsync("https://localhost:7241/api/carts/" + customer.Email);
                    var apiresponse2 = await response2.Content.ReadAsStringAsync();
                    Cart cart = JsonConvert.DeserializeObject<Cart>(apiresponse2)!;
                    LogStateC.CartId = cart.CartId;
                    var cartresponse = await _httpClient.GetAsync("https://localhost:7241/api/cartitems/" + LogStateC.CartId);
                    var cartapiresponse = await cartresponse.Content.ReadAsStringAsync();
                    List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(cartapiresponse)!;
                    if (items.Count() > 0)
                    {
                        List<Food> foods = new List<Food>();
                        foreach (var item in items)
                        {
                            var foodresponse = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + item.FoodCode);
                            var foodapiresponse = await foodresponse.Content.ReadAsStringAsync();
                            Food f = JsonConvert.DeserializeObject<Food>(foodapiresponse)!;
                            f.Quantity = item.Quantity;
                            foods.Add(f);
                        }
                        SaveJsonToSection.SaveObject(HttpContext, "cart", foods);
                    }
                    else
                    {
                        SaveJsonToSection.SaveObject(HttpContext, "cart", new List<Food>());
                        TempData["Log/Res"] = true;
                }
                    }
            }
            else
            {

                HttpContext.Session.SetString("CustomerEmail", customer.Email);
                LogStateC.IsLoggedIn = true;
                LogStateC.Email = HttpContext.Session.GetString("CustomerEmail")!;
                LogStateC.Username = CutStringBeforeAt(LogStateC.Email);
                var response2 = await _httpClient.GetAsync("https://localhost:7241/api/carts/" + customer.Email);
                var apiresponse2 = await response2.Content.ReadAsStringAsync();
                Cart cart = JsonConvert.DeserializeObject<Cart>(apiresponse2)!;
                LogStateC.CartId = cart.CartId;
                var cartresponse = await _httpClient.GetAsync("https://localhost:7241/api/cartitems/" + LogStateC.CartId);
                var cartapiresponse = await cartresponse.Content.ReadAsStringAsync();
                List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(cartapiresponse)!;
                if (items.Count() > 0)
                {
                    List<Food> foods = new List<Food>();
                    foreach (var item in items)
                    {
                        var foodresponse = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + item.FoodCode);
                        var foodapiresponse = await foodresponse.Content.ReadAsStringAsync();
                        Food f = JsonConvert.DeserializeObject<Food>(foodapiresponse)!;
                        f.Quantity = item.Quantity;
                        foods.Add(f);
                    }
                    SaveJsonToSection.SaveObject(HttpContext, "cart", foods);
                }
                else
                {
                    SaveJsonToSection.SaveObject(HttpContext, "cart", new List<Food>());
                }
                TempData["Newbie"] = true;
            }
        }

        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("CustomerEmail") == null)
            {
                LogStateC.IsLoggedIn = false;
                LogStateC.Email = string.Empty;
                LogStateC.CartId = 0;
                LogStateC.Username = string.Empty;
            }
            if(LogStateC.NeedCreate)
            {
               await CreateNewCustomers();
            }
            List<Food> foods = await GetFoods();
            List<FoodCategory> categories = await GetCategories();
            ViewBag.Categories = categories;
            if(foods.Count() > 0)
            {
                return View(foods.Where(x => x.Left > 0).OrderByDescending(x => x.Sold).Take(6));
            } else
            {
                return View(new List<Food>());
            }

        }

        public IActionResult Register()
        {
            return View();
        }

        private static string CutStringBeforeAt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input string cannot be null or empty", nameof(input));
            }

            int atIndex = input.IndexOf('@');
            if (atIndex == -1)
            {
                throw new ArgumentException("Input string does not contain '@' character", nameof(input));
            }

            return input.Substring(0, atIndex);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Customer customer)
        {
            if(ModelState.IsValid)
            {
                customer.Email = customer.Email.Trim();
                customer.PassWord = customer.PassWord.Trim();
                customer.ConfirmPassWord = customer.ConfirmPassWord.Trim(); 
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7241/api/customers", content);
                var apiresponse = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<bool>(apiresponse) == false)
                {
                    var response3 = await _httpClient.PutAsync("https://localhost:7241/api/login/customers", content);
                    var apiresponse3 = await response3.Content.ReadAsStringAsync();
                    if (JsonConvert.DeserializeObject<bool>(apiresponse3))
                    {
                        HttpContext.Session.SetString("CustomerEmail", customer.Email);
                        LogStateC.IsLoggedIn = true;
                        LogStateC.Email = HttpContext.Session.GetString("CustomerEmail")!;
                        LogStateC.Username = CutStringBeforeAt(LogStateC.Email);
                        var response2 = await _httpClient.GetAsync("https://localhost:7241/api/carts/" + customer.Email);
                        var apiresponse2 = await response2.Content.ReadAsStringAsync();
                        Cart cart = JsonConvert.DeserializeObject<Cart>(apiresponse2)!;
                        LogStateC.CartId = cart.CartId;
                        var cartresponse = await _httpClient.GetAsync("https://localhost:7241/api/cartitems/" + LogStateC.CartId);
                        var cartapiresponse = await cartresponse.Content.ReadAsStringAsync();
                        List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(cartapiresponse)!;
                        if (items.Count() > 0)
                        {
                            List<Food> foods = new List<Food>();
                            foreach (var item in items)
                            {
                                var foodresponse = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + item.FoodCode);
                                var foodapiresponse = await foodresponse.Content.ReadAsStringAsync();
                                Food f = JsonConvert.DeserializeObject<Food>(foodapiresponse)!;
                                f.Quantity = item.Quantity;
                                foods.Add(f);
                            }
                            SaveJsonToSection.SaveObject(HttpContext, "cart", foods);
                        }
                        else
                        {
                            SaveJsonToSection.SaveObject(HttpContext, "cart", new List<Food>());
                        }
                        TempData["Log/Res"] = true;
                        return RedirectToAction("Index");
                    } else
                    {
                            TempData["Error"] = "Wrong Email or Password, Please try again !";
                            return RedirectToAction("Register");
                    }
                }
                else
                {
                   
                    HttpContext.Session.SetString("CustomerEmail", customer.Email);
                    LogStateC.IsLoggedIn = true;
                    LogStateC.Email = HttpContext.Session.GetString("CustomerEmail")!;
                    LogStateC.Username = CutStringBeforeAt(LogStateC.Email);
                    var response2 = await _httpClient.GetAsync("https://localhost:7241/api/carts/" + customer.Email);
                    var apiresponse2 = await response2.Content.ReadAsStringAsync();
                    Cart cart = JsonConvert.DeserializeObject<Cart>(apiresponse2)!;
                    LogStateC.CartId = cart.CartId;
                    var cartresponse = await _httpClient.GetAsync("https://localhost:7241/api/cartitems/" + LogStateC.CartId);
                    var cartapiresponse = await cartresponse.Content.ReadAsStringAsync();
                    List<CartItem> items = JsonConvert.DeserializeObject<List<CartItem>>(cartapiresponse)!;
                    if (items.Count() > 0)
                    {
                        List<Food> foods = new List<Food>();
                        foreach (var item in items)
                        {
                            var foodresponse = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + item.FoodCode);
                            var foodapiresponse = await foodresponse.Content.ReadAsStringAsync();
                            Food f = JsonConvert.DeserializeObject<Food>(foodapiresponse)!;
                            f.Quantity = item.Quantity;
                            foods.Add(f);
                        }
                        SaveJsonToSection.SaveObject(HttpContext, "cart", foods);
                    }
                    else
                    {
                        SaveJsonToSection.SaveObject(HttpContext, "cart", new List<Food>());
                    }
                    TempData["Newbie"] = true;
                    return RedirectToAction("AddressRegister");
                }
               
            } 
            else
            {
                return RedirectToAction("Register");
            }
        }

        public IActionResult About()
        {
            return View();
        }

        private static int inforId = 0;

        public async Task<IActionResult> Payment()
        {
            List<Food> foods = SaveJsonToSection.GetObject<List<Food>>(HttpContext, "cart");
            var response = await _httpClient.GetAsync("https://localhost:7241/api/infor/customers/" + LogStateC.Email);
            var apiresponse = await response.Content.ReadAsStringAsync();
            List<CustomerInformation> infor = JsonConvert.DeserializeObject<List<CustomerInformation>>(apiresponse)!;
            if(infor == null || infor.Count() == 0)
            {
                TempData["Newbie"] = true;
                return RedirectToAction("AddressRegister");
            }
            if(inforId == 0)
            {
                inforId = infor.Take(1).Select(x => x.CInforId).FirstOrDefault();
            }
            ViewBag.Infor = infor.Where(x => x.CInforId == inforId).FirstOrDefault();
            return View(foods);
        }

        public async Task<IActionResult> Orders(string state)
        {
            if(LogStateC.IsLoggedIn)
            {
                if (string.IsNullOrEmpty(state))
                {
                    state = "Not delivered";
                }
                var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/" + state.Trim());
                var apiresponse = await response.Content.ReadAsStringAsync();
                List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(apiresponse)!;
                return View(orders.Where(x => x.CustomerEmail == LogStateC.Email));
            } else
            {
                return View(new List<Order>());
            }
        }

        public async Task<IActionResult> Directory()
        {
            var categories = await GetCategories();
            return View(categories);
        }

        private static string savcode = string.Empty;

        public async Task<IActionResult> Products(int sort, string code)
        {
            var foods = await GetFoods();
            if(string.IsNullOrEmpty(code))
            {
                code = savcode;
            } else
            {
                savcode = code;
            }
            foods = foods.Where(x => x.FCategoryCode == code).ToList();
            switch (sort)
            {
                case 1:
                   foods = foods.OrderBy(x => x.CurrentPrice).ToList();
                   break;
                case 2:
                    foods = foods.OrderByDescending(x => x.CurrentPrice).ToList();
                    break;
                case 3:
                    foods = foods.Where(x => x.FTypeCode == "931K").ToList();
                    break;
                case 4:
                    foods = foods.Where(x => x.FTypeCode == "555F").ToList();
                    break;
            }
                
            return View(foods.Where(x => x.Left > 0));
        }

        public async Task<IActionResult> Search(string searchstr)
        {
            var foods = await GetFoods();
            var result = foods.Where(x => x.FoodName.Contains(searchstr, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(result.Where(x => x.Left > 0));
        }

        public IActionResult Logout()
        {
            LogStateC.IsLoggedIn = false;
            LogStateC.Email = string.Empty;
            LogStateC.Username = string.Empty;
            LogStateC.CartId = 0;
            HttpContext.Session.Remove("CustomerEmail");
            HttpContext.Session.Remove("cart");
            return RedirectToAction("Index");
        }

        //Update information of cart item
        private async Task<IActionResult> UpdateCartItem(CartItem item)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://localhost:7241/api/cartitems", content);
            return NoContent();
        }
        private async Task<IActionResult> AddNewCartItem(CartItem item)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7241/api/cartitems", content);
            return NoContent();
        }

        public async Task<IActionResult> AddToCart(string id, int quantity)
        {
            if (LogStateC.CartId > 0)
            {
                CartItem item = new CartItem();
                List<Food> foods = SaveJsonToSection.GetObject<List<Food>>(HttpContext, "cart");
                //Get Food Infor
                var foodresponse = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + id);
                var foodapiresponse = await foodresponse.Content.ReadAsStringAsync();
                Food food = JsonConvert.DeserializeObject<Food>(foodapiresponse)!;
                //Check Exist Food
                int index = -1;
                for(int i = 0; i < foods.Count; i++)
                {
                    if (foods[i].FoodCode == id)
                    {
                        index = i;
                    }
                }
                if (index != -1)
                {
                    foods[index].Quantity += quantity;
                    item.Quantity = (int)foods[index].Quantity!;
                    item.CartId = LogStateC.CartId;
                    item.FoodCode = foods[index].FoodCode!;
                    await UpdateCartItem(item);
                }
                else
                {
                    food.Quantity = quantity;
                    foods.Add(food);
                    item.Quantity = (int)food.Quantity;
                    item.CartId = LogStateC.CartId;
                    item.FoodCode = food.FoodCode!;
                    await AddNewCartItem(item);
                }
                
                SaveJsonToSection.SaveObject(HttpContext, "cart", foods);
                ViewBag.Cart = SaveJsonToSection.GetObject<List<Food>>(HttpContext, "cart");
            }
            return Json(new {quantity = quantity});
        }

        private async Task<IActionResult> RemoveItemInCart(int id, string code)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7241/api/cartitems/{id}/{code}");
            return NoContent();
        }
        public async Task<IActionResult> RemoveItem(string code)
        {
            List<Food> foods = SaveJsonToSection.GetObject<List<Food>>(HttpContext, "cart");
            var d = foods.Where(x => x.FoodCode == code).FirstOrDefault();
            foods.Remove(d!);
            await RemoveItemInCart(LogStateC.CartId, code);
            SaveJsonToSection.SaveObject(HttpContext, "cart", foods);
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> DoPayment(string comment, string method)
        {
            Order order = new Order();
            order.Comment = comment;
            order.PaymentMethod = method;
            order.OrderDate = DateTime.Now;
            order.State = "Not delivered";
            order.CInforId = inforId;
            order.CustomerEmail = LogStateC.Email;
            StringContent content = new StringContent(JsonConvert.SerializeObject(order), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7241/api/orders", content);
            SaveJsonToSection.SaveObject(HttpContext, "cart", new List<Food>());
            TempData["Payment"] = true;
            return RedirectToAction("Index");
        }

        public IActionResult Cart()
        {
            if (SaveJsonToSection.GetObject<List<Food>>(HttpContext, "cart") != null)
            {
                List<Food> foods = SaveJsonToSection.GetObject<List<Food>>(HttpContext, "cart");
                return View(foods);
            }
            else
            {
                return View(new List<Food>());
            }
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/int/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync()!;
            Order order = JsonConvert.DeserializeObject<Order>(apiresponse)!;
            ViewBag.State = order.State;
            ViewBag.PaymentMethod = order.PaymentMethod;
            var addrresponse = await _httpClient.GetAsync("https://localhost:7241/api/orders/address/" + order.CInforId);
            var addrapiresponse = await addrresponse.Content.ReadAsStringAsync()!;
            ViewBag.Address = JsonConvert.DeserializeObject<CustomerInformation>(addrapiresponse);
            var itemsresponse = await _httpClient.GetAsync("https://localhost:7241/api/orders/items/" + order.OrderId);
            var itemsapiresponse = await itemsresponse.Content.ReadAsStringAsync();
            List<OrderItem> ordersi = JsonConvert.DeserializeObject<List<OrderItem>>(itemsapiresponse)!;
            List<Food> foods = new List<Food>();
            foreach(var item in ordersi)
            {
                var fresponse = await _httpClient.GetAsync("https://localhost:7241/api/foods/" + item.FoodCode);
                var apifresponse = await fresponse.Content.ReadAsStringAsync()!;
                Food f = JsonConvert.DeserializeObject<Food>(apifresponse)!;
                f.Quantity = item.Quantity;
                f.Rated = item.Rated;
                f.CurrentPrice = item.UnitPrice;
                f.OrderId = item.OrderId;
                foods.Add(f);
            }
            return View(foods);
        }

        public IActionResult AddressRegister()
        {
            return View(); 
        }

        public async Task<IActionResult> Informations()
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/infor/customers/" + LogStateC.Email);
            var apiresponse = await response.Content.ReadAsStringAsync();
            List<CustomerInformation> informations = JsonConvert.DeserializeObject<List<CustomerInformation>>(apiresponse)!;
            if(LogStateC.IsLoggedIn)
            {
                if (inforId == 0)
                {
                    inforId = informations.Take(1).Select(x => x.CInforId).FirstOrDefault();
                }
                ViewBag.InforId = inforId;
                return View(informations);
            } else
            {
                return View(new List<CustomerInformation>());
            }
        }

        public IActionResult DefaultAddress(int id)
        {
            inforId = id;
            return RedirectToAction("Informations");
        }

        public async Task<IActionResult> DeleteInformation(int id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7241/api/orders/address/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            CustomerInformation information = JsonConvert.DeserializeObject<CustomerInformation>(apiresponse)!;
            return View(information);
        }

        [HttpPost]
        public async Task<IActionResult> deleteInformation(int id)
        {
            var response = await _httpClient.DeleteAsync("https://localhost:7241/api/infor/customers/" + id);
            var apiresponse = await response.Content.ReadAsStringAsync();
            if(JsonConvert.DeserializeObject<bool>(apiresponse))
            {
                TempData["DSucess"] = true;
                return RedirectToAction("Informations");
            } else
            {
                return Error();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddressRegister(CustomerInformation information)
        {
            if(ModelState.IsValid)
            {
                information.CustomerEmail = LogStateC.Email;
                information.Address = $"{information.street}, {information.district}, {information.city}";
                StringContent content = new StringContent(JsonConvert.SerializeObject(information), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7241/api/infor/customers", content);
                TempData["AddRes"] = true;
                return RedirectToAction("Index");
            } else
            {
                return RedirectToAction("AddressRegister");
            }
        }

        public async Task<IActionResult> Rate(string code, int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7241/api/foods/{code}");
            var apiresponse = await response.Content.ReadAsStringAsync();
            Food food = JsonConvert.DeserializeObject<Food>(apiresponse)!;
            food.OrderId = id;
            return View(food);
        }

        [HttpPost]
        public async Task<IActionResult> RateConfirm(int value, string FoodCode, int OrderId)
        {
            Rating rating = new Rating();
            rating.Value = value;
            rating.FoodCode = FoodCode;
            rating.OrderId = OrderId;
            rating.CustomerEmail = LogStateC.Email;
            StringContent content = new StringContent(JsonConvert.SerializeObject(rating), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7241/api/rates", content);
            return RedirectToAction("Orders", new {state = "Delivered"});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
