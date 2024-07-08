using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class RatesAPIController : ControllerBase
    {
        private readonly IAddable<Rating> _addsvc;
        public RatesAPIController(IAddable<Rating> addsvc)
        {
            _addsvc = addsvc; 
        }

        [HttpPost]
        public async Task<bool> PostRate(Rating rating)
        {
            return await _addsvc.AddNewData(rating);
        }
    }
}
