using API.Context;
using API.Services.Interfaces;
using UI.Models;
using API.Services.Implement;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implement
{
    public class AdminLoginSvc : ILoginSvc<Admin>
    {
        private readonly FoodShopDBContext _dbContext;

        public AdminLoginSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Login(Admin entity)
        {
            var admin = await _dbContext.admins.Where(x => x.Email == entity.Email).FirstOrDefaultAsync();
            if(admin != default)
            {
                if(admin.Password == AuthencationDataSvc.EncryptionPassword(entity.Password))
                {
                    admin.IsOnl = true;
                    await _dbContext.SaveChangesAsync();
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }

        public async Task Logout(string email)
        {
            var admin = await _dbContext.admins.Where(x => x.Email == email).FirstOrDefaultAsync();
            if(admin != default)
            {
                admin.IsOnl = false;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
