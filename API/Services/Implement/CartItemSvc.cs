using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Services.Implement
{
    public class CartItemSvc : IAddable<CartItem>, IReadableHasWhere<int, CartItem>, IDeletable<List<CartItem>, CartItem>, IEditable<CartItem>
    {
        private readonly FoodShopDBContext _dbContext;
        public CartItemSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task<bool> AddNewData(CartItem entity)
        {
            try
            {
                await _dbContext.cartItem.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteData(List<CartItem> items)
        {
            try
            {
                if (items != null)
                {
                    _dbContext.cartItem.RemoveRange(items);
                    await _dbContext.SaveChangesAsync();
                    return true;
                } else
                {
                    return false;
                }
            } catch
            {
                return false;
            }
        }

        public async Task<bool> EditData(CartItem entity)
        {
            try
            {
                var item = await _dbContext.cartItem.Where(x => x.CartId == entity.CartId).FirstOrDefaultAsync();
                item!.Quantity = entity.Quantity;
                item.CartId = entity.CartId;
                item.FoodCode = entity.FoodCode;
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }

        }

        public async  Task<IEnumerable<CartItem>> ReadDatasHasW(int key)
        {
            return await _dbContext.cartItem.Where(x => x.CartId == key).ToListAsync();
        }
    }
}
