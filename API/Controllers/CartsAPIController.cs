using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartsAPIController : ControllerBase
    {
        private readonly ILookupSvc<string, Cart> _lookupsvc;
        public CartsAPIController(ILookupSvc<string, Cart> lookupsvc)
        {
            _lookupsvc = lookupsvc;
        }

        [HttpGet("{email}")]
        public async Task<Cart> GetCart(string email)
        {
           return await _lookupsvc.GetDataByKey(email);
        }
    }
}
