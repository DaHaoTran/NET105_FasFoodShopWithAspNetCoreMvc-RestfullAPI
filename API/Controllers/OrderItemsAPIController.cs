using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/orderitems")]
    [ApiController]
    public class OrderItemsAPIController : ControllerBase
    {
        private readonly IAddable<OrderItem> _addsvc;
        public OrderItemsAPIController(IAddable<OrderItem> addsvc)
        {
            _addsvc = addsvc;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> _PostItems(OrderItem item)
        {
            return await _addsvc.AddNewData(item);
        }
    }
}
