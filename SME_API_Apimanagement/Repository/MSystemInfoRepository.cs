using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SME_API_Apimanagement.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME_API_Apimanagement.Repository
{
    public class MSystemInfoRepository : IMSystemInfoRepository
    {
        private readonly ApiMangeDBContext _context;

        public MSystemInfoRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MSystemInfo>> GetAllAsync()
        {
            try
            {
                return await _context.Set<MSystemInfo>().Where(x => x.FlagDelete == "N").ToListAsync();
            }
            catch
            {
                return new List<MSystemInfo>();
            }
        }

        public async Task<MSystemInfo?> GetByIdAsync(string sysCode)
        {
            try
            {
                return await _context.Set<MSystemInfo>().AsNoTracking().FirstOrDefaultAsync(x => x.SystemCode == sysCode);
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> AddAsync(MSystemInfo entity)
        {
            try
            {
                _context.Set<MSystemInfo>().Add(entity);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateAsync(MSystemInfo entity)
        {
            try
            {
                _context.Set<MSystemInfo>().Update(entity);
                if (!string.IsNullOrEmpty(entity.ApiKey)) 
                {
                    _context.Entry(entity).Property(x => x.ApiKey).IsModified = true;
                }
                if (!string.IsNullOrEmpty(entity.ApiPassword))
                    _context.Entry(entity).Property(x => x.ApiPassword).IsModified = true;
                if (!string.IsNullOrEmpty(entity.ApiUrlProdInbound))
                    _context.Entry(entity).Property(x => x.ApiUrlProdInbound).IsModified = true;
                if (!string.IsNullOrEmpty(entity.ApiUrlUatInbound))
                    _context.Entry(entity).Property(x => x.ApiUrlUatInbound).IsModified = true;
                if (!string.IsNullOrEmpty(entity.UpdateBy))
                    _context.Entry(entity).Property(x => x.UpdateBy).IsModified = true;
            
                if (!string.IsNullOrEmpty(entity.Note))
                    _context.Entry(entity).Property(x => x.Note).IsModified = true;
                if (!string.IsNullOrEmpty(entity.FlagDelete))
                    _context.Entry(entity).Property(x => x.FlagDelete).IsModified = true;
                _context.Entry(entity).Property(x => x.UpdateDate).IsModified = true;
                _context.Entry(entity).Property(x => x.FlagActive).IsModified = true;

                return await _context.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var item = await _context.Set<MSystemInfo>().FindAsync(id);
                if (item == null) return 0;
                item.FlagDelete = "Y";
                item.UpdateDate = DateTime.UtcNow;
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }
    }

    public interface IMSystemInfoRepository
    {
        Task<IEnumerable<MSystemInfo>> GetAllAsync();
        Task<MSystemInfo?> GetByIdAsync(string id);
        Task<int> AddAsync(MSystemInfo entity);
        Task<int> UpdateAsync(MSystemInfo entity);
        Task<int> DeleteAsync(int id);
    }
}