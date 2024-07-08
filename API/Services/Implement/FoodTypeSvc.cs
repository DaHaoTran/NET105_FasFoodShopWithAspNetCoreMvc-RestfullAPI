using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implement
{
    public class FoodTypeSvc : IReadable<FoodType>
    {
        private readonly FoodShopDBContext _dbContext;
        public FoodTypeSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FoodType>> ReadDatas()
        {
            return await _dbContext.foodType.ToListAsync();
        }
    }
}
