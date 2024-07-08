using UI.Models;
using API.Services.Implement;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersAPIController : ControllerBase
    {
        private readonly IReadableHasWhere<string, Order> _readwsvc;
        private readonly ILookupSvc<int, Order> _lookupSvc;
        private readonly IEditable<Order> _editsvc;
        private readonly ILookupSvc<int, CustomerInformation> _cuslookupsvc;
        private readonly IReadableHasWhere<int, OrderItem> _orderIreadsvc;
        private readonly IAddable<Order> _addsvc;
        public OrdersAPIController(IReadableHasWhere<string, Order> readwsvc,
            ILookupSvc<int, Order> lookupsvc,
            IEditable<Order> editsvc,
            ILookupSvc<int, CustomerInformation> cuslookupsvc,
            IReadableHasWhere<int, OrderItem> orderIreadsvc,
            IAddable<Order> addsvc) 
        {
            _readwsvc = readwsvc;
            _lookupSvc = lookupsvc;
            _editsvc = editsvc;
            _cuslookupsvc = cuslookupsvc;
            _orderIreadsvc = orderIreadsvc;
            _addsvc = addsvc;
        }

        // GET: api/orders/state
        [HttpGet("{state}")]
        public async Task<IEnumerable<Order>> Getorders(string state)
        {
            return await _readwsvc.ReadDatasHasW(state);
        }

        // GET: api/orders/address/5
        [HttpGet("address/{id}")]
        public async Task<ActionResult<CustomerInformation>> GetAddress(int id)
        {
            var address = await _cuslookupsvc.GetDataByKey(id);
            return address;
        }

        // GET: api/orders/items/5
        [HttpGet("items/{id}")]
        public async Task<IEnumerable<OrderItem>> GetItems(int id)
        {
            return await _orderIreadsvc.ReadDatasHasW(id);
        }

        // PUT: api/orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<bool> PutOrder(int id, Order order)
        {
            bool done = false;
            if (id != order.OrderId)
            {
                return false;
            }
            else
            {
                done = await _editsvc.EditData(order);
            }
            return done;
        }

        // GET: api/orders/string/email
        [HttpGet("string/{email}")]
        public async Task<ActionResult<Order>> GetOrderByString(string email)
        {
            var order = await _lookupSvc.GetDataByString(email);
            return order;
        }

        [HttpGet("int/{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            return await _lookupSvc.GetDataByKey(id);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostOrder(Order order)
        {
            return await _addsvc.AddNewData(order);
        } 
    }
}
