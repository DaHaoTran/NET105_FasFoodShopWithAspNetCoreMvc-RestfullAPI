using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/infor/customers")]
    [ApiController]
    public class CustomerInformationsAPIController : ControllerBase
    {
        private readonly ILookupSvc<int, CustomerInformation> _lookupSvc;
        private readonly IAddable<CustomerInformation> _addsvc;
        private readonly IReadable<CustomerInformation> _readsvc;
        private readonly IDeletable<int, CustomerInformation> _deletesvc;
        private readonly IReadableHasWhere<string, CustomerInformation> _readWsvc;
        public CustomerInformationsAPIController(ILookupSvc<int, CustomerInformation> lookupSvc,
                IAddable<CustomerInformation> addsvc,
                IReadable<CustomerInformation> readsvc,
                IDeletable<int, CustomerInformation> deletesvc,
                IReadableHasWhere<string, CustomerInformation> readWsvc)
        {
            _lookupSvc = lookupSvc;
            _addsvc = addsvc;
            _readsvc = readsvc;
            _deletesvc = deletesvc;
            _readWsvc = readWsvc;
        }

        //[HttpGet("{email}")]
        //public async Task<CustomerInformation> GetInformation(string email)
        //{
        //    return await _lookupSvc.GetDataByString(email);
        //}

        [HttpPost]
        public async Task<bool> PostInformation(CustomerInformation information)
        {
            return await _addsvc.AddNewData(information);
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerInformation>> GetInformations()
        {
            return await _readsvc.ReadDatas();
        }

        [HttpGet("{email}")]
        public async Task<IEnumerable<CustomerInformation>> GetInformations(string email)
        {
            return await _readWsvc.ReadDatasHasW(email);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteInformation(int id)
        {
            return await _deletesvc.DeleteData(id);
        }
    }
}
