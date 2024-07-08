using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/cartitems")]
    [ApiController]
    public class CartitemsController : ControllerBase
    {
        private readonly IAddable<CartItem> _addsvc;
        private readonly IReadableHasWhere<int, CartItem> _readWsvc;
        private readonly IDeletable<List<CartItem>,  CartItem> _deletesvc;
        private readonly IEditable<CartItem> _editsvc;
        public CartitemsController(IAddable<CartItem> addsvc, IReadableHasWhere<int, CartItem> readWsvc, 
                IDeletable<List<CartItem>, CartItem> deletesvc, IEditable<CartItem> editsvc)
        {
            _addsvc = addsvc;
            _readWsvc = readWsvc;
            _deletesvc = deletesvc;
            _editsvc = editsvc;
        }

        [HttpPost]
        public async Task<bool> PostItem(CartItem item)
        {
            return await _addsvc.AddNewData(item);
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<CartItem>> GetItems(int id)
        {
            return await _readWsvc.ReadDatasHasW(id);
        }

        [HttpDelete("{id}/{code}")]
        public async Task<bool> Deleteitem(int id, string code)
        {
            var items = await _readWsvc.ReadDatasHasW(id);
            List<CartItem> item = items.Where(x => x.FoodCode == code).ToList();
            if(item != null)
            {
                return await _deletesvc.DeleteData(item);
            } else
            {
                return false;
            }
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteItems(int id)
        {
            var items = await _readWsvc.ReadDatasHasW(id);
            return await _deletesvc.DeleteData(items.ToList());
        }

        [HttpPut]
        public async Task<bool> PutItem(CartItem item)
        {
            return await _editsvc.EditData(item);
        }
    }
}
