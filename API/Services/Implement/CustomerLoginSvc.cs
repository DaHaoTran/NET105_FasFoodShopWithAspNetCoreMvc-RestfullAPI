using API.Context;
using API.Services.Interfaces;
using UI.Models;
using API.Services.Implement;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implement
{
    public class CustomerLoginSvc : ILoginSvc<Customer>
    {
        private readonly FoodShopDBContext _dbContext;

        public CustomerLoginSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Login(Customer entity)
        {
            var customer = await _dbContext.customer.Where(x => x.Email == entity.Email).FirstOrDefaultAsync();
            if (customer != default)
            {
                if (customer.PassWord == AuthencationDataSvc.EncryptionPassword(entity.PassWord))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Task Logout(string email)
        {
            throw new NotImplementedException();
        }
    }
}
