using API.Services.Interfaces;
using UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/login/admins")]
    [ApiController]
    public class LoginAdminAPIController : ControllerBase
    {
        private readonly ILoginSvc<Admin> _loginSvc;
        public LoginAdminAPIController(ILoginSvc<Admin> loginSvc)
        {
            _loginSvc = loginSvc;
        }

        // api/login/admins
        [HttpPut]
        public async Task<bool> LoginAdmin(Admin admin)
        {
            if(await _loginSvc.Login(admin))
            {
                return true;
            } else
            {
                return false;
            }
        }

        // api/login/admins/email
        [HttpGet("{email}")]
        public async Task LogoutAdmin(string email)
        {
            await _loginSvc.Logout(email);
        }
    }
}
