using API.Context;
using UI.Models;
using API.Services.Implement;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Services.Implement
{
    public class FoodCategorySvc : ILookupSvc<string, FoodCategory>, IAddable<FoodCategory>, IEditable<FoodCategory>, IDeletable<string, FoodCategory>, IReadable<FoodCategory>
    {
        private readonly FoodShopDBContext _dbContext;
        public FoodCategorySvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewData(FoodCategory entity)
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    var code = AuthencationDataSvc.GenerateNewCode(4);
                    while (_dbContext.admins.Any(x => x.AdminCode == code))
                    {
                        code = AuthencationDataSvc.GenerateNewCode(4);
                    }
                    entity.FCategoryCode = code;
                });
                task.Wait();
                await _dbContext.foodCategory.AddAsync(entity);
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
                _dbContext.foodCategory.Remove(find);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditData(FoodCategory entity)
        {
            try
            {
                var find = await GetDataByKey(entity.FCategoryCode!);
                Task task = Task.Run(() =>
                {
                    find.CategoryName = entity.CategoryName;
                    find.Image = entity.Image;
                });
                task.Wait();
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public Task<FoodCategory> GetDataByKey(string key)
        {
            return _dbContext.foodCategory.Where(x => x.FCategoryCode == key).FirstOrDefaultAsync()!;
        }

        public Task<FoodCategory> GetDataByString(string str)
        {
            return _dbContext.foodCategory.Where(x => x.CategoryName == str.Trim()).FirstOrDefaultAsync()!;
        }

        public async Task<IEnumerable<FoodCategory>> ReadDatas()
        {
            return await _dbContext.foodCategory.ToListAsync();
        }
    }
}
