using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Services.Implement
{
    public class FoodSvc : ILookupSvc<string, Food>, IAddable<Food>, IEditable<Food>, IDeletable<string, Food>, IReadable<Food>
    {
        private readonly FoodShopDBContext _dbContext;
        public FoodSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewData(Food entity)
        {
            try
            {
                Task task = Task.Run(async () =>
                {
                    var admin = await _dbContext.admins.Where(x => x.Email == entity.AdminCode).FirstOrDefaultAsync();
                    entity.AdminCode = admin!.AdminCode;
                    var code = AuthencationDataSvc.GenerateNewCode(5);
                    while (_dbContext.admins.Any(x => x.AdminCode == code))
                    {
                        code = AuthencationDataSvc.GenerateNewCode(5);
                    }
                    entity.FoodCode = code;
                    entity.Sold = 0;
                    entity.PreviousPrice = entity.CurrentPrice;
                    entity.FCategoryCode = await _dbContext.foodCategory.Where(x => x.CategoryName == entity.FCategoryCode).Select(x => x.FCategoryCode).FirstOrDefaultAsync()!;
                    entity.FTypeCode = await _dbContext.foodType.Where(x => x.TypeName == entity.FTypeCode).Select(x => x.FTypeCode).FirstOrDefaultAsync()!;
                    //Get Admin test
                    //entity.AdminCode = await _dbContext.admins.Select(x => x.AdminCode).Take(1).FirstOrDefaultAsync()!;
                });
                task.Wait();
                await _dbContext.foods.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteData(string key)
        {
            try
            {
                var find = await GetDataByKey(key);
                _dbContext.foods.Remove(find);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> EditData(Food entity)
        {
            try
            {
                var find = await GetDataByKey(entity.FoodCode);
                Task task = Task.Run(async () =>
                {
                    var admin = await _dbContext.admins.Where(x => x.Email == entity.AdminCode).FirstOrDefaultAsync();
                    find.AdminCode = admin!.AdminCode;
                    find.FoodName = entity.FoodName;
                    find.PreviousPrice = find.CurrentPrice;
                    find.CurrentPrice = entity.CurrentPrice;
                    if(find.Sold == null)
                    {
                        find.Sold = 0;
                    }
                    else
                    {
                        find.Sold = entity.Sold;
                    }
                    find.Left = entity.Left;
                    find.Image = entity.Image;
                    find.FCategoryCode = await _dbContext.foodCategory.Where(x => x.CategoryName == entity.FCategoryCode).Select(x => x.FCategoryCode).FirstOrDefaultAsync()!;
                    find.FTypeCode = await _dbContext.foodType.Where(x => x.TypeName == entity.FTypeCode).Select(x => x.FTypeCode).FirstOrDefaultAsync()!;
                });
                task.Wait();
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public Task<Food> GetDataByKey(string key)
        {
            return _dbContext.foods.Where(x => x.FoodCode == key).FirstOrDefaultAsync()!;
        }

        public Task<Food> GetDataByString(string str)
        {
            return _dbContext.foods.Where(x => x.FoodName == str.Trim()).FirstOrDefaultAsync()!;
        }

        public async Task<IEnumerable<Food>> ReadDatas()
        {
            return await _dbContext.foods.ToListAsync();
        }
    }
}
