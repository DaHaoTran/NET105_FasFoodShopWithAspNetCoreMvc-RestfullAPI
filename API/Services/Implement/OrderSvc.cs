using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Services.Implement
{
    public class OrderSvc : ILookupSvc<int, Order>, IEditable<Order>, IReadableHasWhere<string, Order>, IAddable<Order>
    {
        private readonly FoodShopDBContext _dbContext;
        public OrderSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewData(Order entity)
        {
            try
            {
                await _dbContext.order.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> EditData(Order entity)
        {
            try
            {
                var find = await GetDataByKey(entity.OrderId);
                find.State = entity.State;
                if(find.State == "Delivered")
                {
                    find.DeliveryDate = DateTime.Now;
                }
                else
                {
                    find.DeliveryDate = null;
                }
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public Task<Order> GetDataByKey(int key)
        {
            return _dbContext.order.Where(x => x.OrderId == key).FirstOrDefaultAsync()!;
        }

        public Task<Order> GetDataByString(string str)
        {
            return _dbContext.order.Where(x => x.CustomerEmail == str.Trim()).FirstOrDefaultAsync()!;
        }

        public async Task<IEnumerable<Order>> ReadDatasHasW(string key) 
        {
            return await _dbContext.order.Where(x => x.State == key.Trim()).ToListAsync();
        }
    }
}
