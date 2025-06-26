using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;

namespace SME_API_Apimanagement.Repository
{
    public class UserManagementRepository
    {
        private readonly ApiMangeDBContext _context;

        public UserManagementRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<List<TEmployeeRole>> GetAllAsync(string buid =null)
        {
            if (buid != null && buid != "")
            {
                return await _context.TEmployeeRoles.Where(e => e.BusinessUnitId == buid).ToListAsync();
              
            }
            else 
            {
                return await _context.TEmployeeRoles.ToListAsync();
            }
           
        }

        public async Task<TEmployeeRole?> GetByIdAsync(int id)
        {
            return await _context.TEmployeeRoles.FindAsync(id);
        }

        public async Task AddAsync(TEmployeeRole entity)
        {
            _context.TEmployeeRoles.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEmployeeRole entity)
        {
            _context.TEmployeeRoles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
          
            try
            {
                var entity = await _context.TEmployeeRoles.FindAsync(id);
                if (entity != null)
                {
                    _context.TEmployeeRoles.Remove(entity);
                    await _context.SaveChangesAsync();
                    
                }
                return 1;
            }
            catch (Exception ex) 
            {
                return 0;
            }
            
        }

        public async Task<TEmployeeRole?> GetByEmpRoleAsync(string EmpId)
        {
            return await _context.TEmployeeRoles.FirstOrDefaultAsync(e => e.EmployeeCode == EmpId);
        }

    }
}