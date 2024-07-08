using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implement
{
    public class OrderItemSvc : IReadableHasWhere<int, OrderItem>, IAddable<OrderItem>
    {
        private readonly FoodShopDBContext _dbContext;
        public OrderItemSvc(FoodShopDBContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewData(OrderItem entity)
        {
            try
            {
                await _dbContext.orderItem.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<OrderItem>> ReadDatasHasW(int key)
        {
            return await _dbContext.orderItem.Where(x => x.OrderId == key).ToListAsync();
        }
    }
}
