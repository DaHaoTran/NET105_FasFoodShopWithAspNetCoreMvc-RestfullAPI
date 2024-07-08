using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implement
{
    public class CartSvc : ILookupSvc<string, Cart>
    {
        private readonly FoodShopDBContext _dbContext;
        public CartSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public Task<Cart> GetDataByKey(string key)
        {
            return _dbContext.cart.Where(x => x.CustomerEmail == key).FirstOrDefaultAsync()!;
        }

        public Task<Cart> GetDataByString(string str)
        {
            throw new NotImplementedException();
        }
    }
}
