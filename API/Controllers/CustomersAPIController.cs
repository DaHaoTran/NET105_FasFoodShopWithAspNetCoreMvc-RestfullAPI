using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersAPIController : ControllerBase
    {
        private readonly IAddable<Customer> _addsvc;
        private readonly IReadable<Customer> _readsvc;
        private readonly ILookupSvc<string, Customer> _lookupsvc;
        private readonly IEditable<Customer> _editsvc;
        private readonly IDeletable<string, Customer> _deletesvc;
        public CustomersAPIController(IAddable<Customer> addsvc,
            IReadable<Customer> readsvc,
            ILookupSvc<string, Customer> lookupsvc,
            IEditable<Customer> editsvc,
            IDeletable<string, Customer> deletesvc)
        {
            _addsvc = addsvc;
            _readsvc = readsvc;
            _lookupsvc = lookupsvc;
            _editsvc = editsvc;
            _deletesvc = deletesvc;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _readsvc.ReadDatas();
        }

        // GET: api/customers/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Customer>> Getcustomer(string code)
        {
            var cus = await _lookupsvc.GetDataByKey(code);

            if (cus == null)
            {
                return NotFound();
            }

            return cus;
        }

        // GET: api/customers/string/name
        [HttpGet("string/{name}")]
        public async Task<ActionResult<Customer>> GetCustomerByString(string name)
        {
            var cus = await _lookupsvc.GetDataByString(name);
            return cus;
        }

        // PUT: api/customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<bool> PutCustomer(string code, Customer cus)
        {
            bool done = false;
            if (code != cus.Email)
            {
                return false;
            }
            else
            {
                done = await _editsvc.EditData(cus);
            }
            return done;
        }

        // POST: api/customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<bool> PostCustomer(Customer cus)
        {
            bool done = await _addsvc.AddNewData(cus);
            return done;
        }

        // DELETE: api/customers/5
        [HttpDelete("{code}")]
        public async Task<bool> DeleteCustomer(string code)
        {
            bool done = await _deletesvc.DeleteData(code);
            return done;
        }
    }
}
