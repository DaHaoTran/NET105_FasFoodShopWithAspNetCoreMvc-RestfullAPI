using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Services.Interfaces;
using UI.Models;

namespace API.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminsAPIController : ControllerBase
    {
        private readonly IAddable<Admin> _addsvc;
        private readonly IReadable<Admin> _readsvc;
        private readonly ILookupSvc<string, Admin> _lookupsvc;
        private readonly IEditable<Admin> _editsvc;
        private readonly IDeletable<string, Admin> _deletesvc;
        public AdminsAPIController(IAddable<Admin> addsvc, 
            IReadable<Admin> readsvc, 
            ILookupSvc<string, Admin> lookupsvc, 
            IEditable<Admin> editsvc,
            IDeletable<string, Admin> deletesvc)
        {
            _addsvc = addsvc;
            _readsvc = readsvc;
            _lookupsvc = lookupsvc;
            _editsvc = editsvc;
            _deletesvc = deletesvc;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<IEnumerable<Admin>> Getadmins()
        {
            return await _readsvc.ReadDatas();
        }

        // GET: api/Admins/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Admin>> GetAdmin(string code)
        {
            var admin = await _lookupsvc.GetDataByKey(code);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // GET: api/Admins/string/email
        [HttpGet("string/{email}")]
        public async Task<ActionResult<Admin>> GetAdminByString(string email)
        {
            var admin = await _lookupsvc.GetDataByString(email);
            return admin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<bool> PutAdmin(string code, Admin admin)
        {
            bool done = false;
            if (code != admin.AdminCode)
            {
                return false;
            } else
            {
                done = await _editsvc.EditData(admin);
            }
            return done;
        }

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<bool> PostAdmin(Admin admin)
        {
            bool done = await _addsvc.AddNewData(admin);
            return done;
        }

        // DELETE: api/Admins/5
        [HttpDelete("{code}")]
        public async Task<bool> DeleteAdmin(string code)
        {
            bool done = await _deletesvc.DeleteData(code);
            return done;
        }

        //private Task<bool> AdminExists(string code)
        //{
        //    return _lookupsvc.CheckDataExists(code);
        //}
    }
}
