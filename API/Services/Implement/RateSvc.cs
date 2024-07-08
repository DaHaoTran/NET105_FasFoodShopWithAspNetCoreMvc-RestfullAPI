using API.Context;
using API.Services.Interfaces;
using UI.Models;

namespace API.Services.Implement
{
    public class RateSvc : IAddable<Rating>
    {
        private readonly FoodShopDBContext _dBContext;
        public RateSvc(FoodShopDBContext dbContext)
        {
            _dBContext = dbContext;
        }

        public async Task<bool> AddNewData(Rating entity)
        {
            try
            {
                await _dBContext.rating.AddAsync(entity);
                await _dBContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }
    }
}
