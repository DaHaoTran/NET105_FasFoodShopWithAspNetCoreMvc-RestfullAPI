using API.Context;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace API.Services.Implement
{
    public class AdminSvc : IReadable<Admin>, IAddable<Admin>, IEditable<Admin>, IDeletable<string, Admin>, ILookupSvc<string, Admin>
    {
        private readonly FoodShopDBContext _dbContext;
        public AdminSvc(FoodShopDBContext dBContext) 
        {
            _dbContext = dBContext;
        }

        public async Task<bool> AddNewData(Admin entity)
        {
            var data = await GetDataByString(entity.Email);
            if (data != default)
            {
                return false;
            } else
            {
                try
                {
                    Task setT = Task.Run(() =>
                    {
                        entity.CreatedDate = DateTime.Now;
                        entity.IsOnl = false;
                        var code = AuthencationDataSvc.GenerateNewCode(5);
                        while (_dbContext.admins.Any(x => x.AdminCode == code))
                        {
                            code = AuthencationDataSvc.GenerateNewCode(5);
                        }
                        entity.AdminCode = code;
                        entity.Password = AuthencationDataSvc.EncryptionPassword(entity.Password);
                    });
                    setT.Wait();
                    await _dbContext.admins.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> DeleteData(string key)
        {
            try
            {
                var find = await GetDataByKey(key);
                if (find.IsOnl != true)
                {
                    return false;
                }
                else
                {
                    _dbContext.admins.Remove(find);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditData(Admin entity)
        {
           if(entity.IsOnl != true)
           {
                try
                {
                    var find = await GetDataByKey(entity.AdminCode!);
                    Task task = Task.Run(() =>
                    {
                        find.Email = entity.Email;
                        find.Password = AuthencationDataSvc.EncryptionPassword(entity.Password);
                    });
                    task.Wait();
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
           }
           return false;
        }

        public Task<Admin> GetDataByKey(string key)
        {
           return _dbContext.admins.Where(x => x.AdminCode == key).FirstOrDefaultAsync()!;
        }

        public Task<Admin> GetDataByString(string str)
        {
            return _dbContext.admins.Where(x => x.Email == str.Trim()).FirstOrDefaultAsync()!;
        }

        public async Task<IEnumerable<Admin>> ReadDatas()
        {
            return await _dbContext.admins.ToListAsync();
        }
    }
}
