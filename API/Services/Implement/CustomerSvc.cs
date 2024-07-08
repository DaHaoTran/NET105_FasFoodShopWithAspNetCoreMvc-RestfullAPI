using API.Context;
using UI.Models;
using API.Services.Implement;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Services.Implement
{
    public class CustomerSvc : ILookupSvc<string, Customer>, IAddable<Customer>, IEditable<Customer>, IDeletable<string, Customer>, IReadable<Customer>
    {
        private readonly FoodShopDBContext _dbContext;
        public CustomerSvc(FoodShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static string CutStringBeforeAt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input string cannot be null or empty", nameof(input));
            }

            int atIndex = input.IndexOf('@');
            if (atIndex == -1)
            {
                throw new ArgumentException("Input string does not contain '@' character", nameof(input));
            }

            return input.Substring(0, atIndex);
        }

        public async Task<bool> AddNewData(Customer entity)
        {
            if(await _dbContext.customer.Where(x => x.Email == entity.Email).FirstOrDefaultAsync() == default)
            {
                try
                {
                    entity.AdminCode = await _dbContext.admins.Where(x => x.Email == entity.AdminCode).Select(x => x.AdminCode).FirstOrDefaultAsync();
                    entity.UserName = CutStringBeforeAt(entity.Email);
                    entity.PassWord = AuthencationDataSvc.EncryptionPassword(entity.PassWord);
                    await _dbContext.customer.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteData(string key)
        {
            try
            {
                var find = await GetDataByKey(key);
                _dbContext.customer.Remove(find);
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> EditData(Customer entity)
        {
            try
            {
                var find = await GetDataByKey(entity.Email);
                Task task = Task.Run(async () =>
                {
                    var admin = await _dbContext.admins.Where(x => x.Email == entity.AdminCode).FirstOrDefaultAsync();
                    find.AdminCode = admin!.AdminCode;
                    find.PassWord = AuthencationDataSvc.EncryptionPassword(entity.PassWord);
                });
                task.Wait();
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                return false;
            }
        }

        public Task<Customer> GetDataByKey(string key)
        {
            return _dbContext.customer.Where(x => x.Email == key).FirstOrDefaultAsync()!;
        }

        public Task<Customer> GetDataByString(string str)
        {
            return _dbContext.customer.Where(x => x.UserName == str.Trim()).FirstOrDefaultAsync()!;
        }

        public async Task<IEnumerable<Customer>> ReadDatas()
        {
            return await _dbContext.customer.ToListAsync();
        }
    }
}
