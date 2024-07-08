using API.Context;
using UI.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implement
{
    public class CustomerInformationSvc : ILookupSvc<int, CustomerInformation>, IAddable<CustomerInformation>, IReadable<CustomerInformation>, IDeletable<int, CustomerInformation>, IReadableHasWhere<string, CustomerInformation>
    {
        private FoodShopDBContext _dbContext;
        public CustomerInformationSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewData(CustomerInformation entity)
        {
            try
            {
                await _dbContext.customerInformation.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteData(int key)
        {
            try
            {
                var find = await GetDataByKey(key);
                _dbContext.customerInformation.Remove(find);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public Task<CustomerInformation> GetDataByKey(int key)
        {
            return _dbContext.customerInformation.Where(x => x.CInforId == key).FirstOrDefaultAsync()!;
        }

        public Task<CustomerInformation> GetDataByString(string str)
        {
            return _dbContext.customerInformation.Where(x => x.CustomerEmail == str).FirstOrDefaultAsync()!;
        }

        public async Task<IEnumerable<CustomerInformation>> ReadDatas()
        {
            return await _dbContext.customerInformation.ToListAsync();
        }

        public async Task<IEnumerable<CustomerInformation>> ReadDatasHasW(string key)
        {
            return await _dbContext.customerInformation.Where(x => x.CustomerEmail == key).ToListAsync();
        }
    }
}
