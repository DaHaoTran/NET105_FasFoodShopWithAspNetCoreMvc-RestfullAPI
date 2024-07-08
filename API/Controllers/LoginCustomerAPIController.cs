using API.Services.Interfaces;
using UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/login/customers")]
    [ApiController]
    public class LoginCustomerAPIController : ControllerBase
    {
        private readonly ILoginSvc<Customer> _loginSvc;
        public LoginCustomerAPIController(ILoginSvc<Customer> loginSvc)
        {
            _loginSvc = loginSvc;
        }

        // api/login/admins
        [HttpPut]
        public async Task<bool> LoginAdmin(Customer customer)
        {
            return await _loginSvc.Login(customer);
        }
    }
}
