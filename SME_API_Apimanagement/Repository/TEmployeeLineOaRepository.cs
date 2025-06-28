using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;

namespace SME_API_Apimanagement.Repository
{
    public class TEmployeeLineOaRepository  
    {
        private readonly ApiMangeDBContext _context;

        public TEmployeeLineOaRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<List<TEmployeeLineOa>> GetAllAsync()
        {
            try
            {
                return await _context.TEmployeeLineOas.ToListAsync();
            }
            catch
            {
                return new List<TEmployeeLineOa>();
            }
        }

        public async Task<TEmployeeLineOa?> GetByIdAsync(string EmployeeId)
        {
            try
            {
                var data = await _context.TEmployeeLineOas.Where(e => e.EmployeeId == EmployeeId).FirstOrDefaultAsync();
                if (data == null)
                {
                    return null;
                }
                return data;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddAsync(TEmployeeLineOa entity)
        {
            try
            {
                _context.TEmployeeLineOas.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEmployeeLineOa entity)
        {
            try
            {
                _context.TEmployeeLineOas.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.TEmployeeLineOas.FindAsync(id);
                if (entity == null) return false;
                _context.TEmployeeLineOas.Remove(entity);
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