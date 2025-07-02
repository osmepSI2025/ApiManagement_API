using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;

namespace SME_API_Apimanagement.Repository
{
    public class TEmployeeMapSystemRepository
    {
        private readonly ApiMangeDBContext _context;

        public TEmployeeMapSystemRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<List<TEmployeeMapSystem>> GetAllAsync()
        {
            try
            {
                return await _context.TEmployeeMapSystems.ToListAsync();
            }
            catch
            {
                return new List<TEmployeeMapSystem>();
            }
        }

        public async Task<TEmployeeMapSystem?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.TEmployeeMapSystems.FindAsync(id);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<TEmployeeMapSystem?>> GetByEmpIdAsync(string empid)
        {
            try
            {
                return await _context.TEmployeeMapSystems.Where(e=>e.EmployeeId == empid).ToListAsync();
            }
            catch
            {
                return null;
            }
        }
      

        public async Task<bool> UpdateAsync(TEmployeeMapSystem entity)
        {
            try
            {
                _context.TEmployeeMapSystems.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string empid)
        {
            try
            {
                var entities = await _context.TEmployeeMapSystems
                    .Where(e => e.EmployeeId == empid)
                    .ToListAsync();

                if (entities == null || entities.Count == 0) return false;

                _context.TEmployeeMapSystems.RemoveRange(entities);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddRangeAsync(List<TEmployeeMapSystem> entity)
        {
            try
            {
                _context.TEmployeeMapSystems.AddRangeAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddAsync(TEmployeeMapSystem entity)
        {
            try
            {
                _context.TEmployeeMapSystems.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}