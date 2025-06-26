using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public class TErrorApiLogRepository : ITErrorApiLogRepository
    {
        private readonly ApiMangeDBContext _context;

        public TErrorApiLogRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<ViewErroApiModels> GetAllAsync(TErrorApiLogModels xmodel)
        {
            var xresult = new ViewErroApiModels();
            try
            {
                var query = (from e in _context.TErrorApiLogs
                             join s in _context.MSystems on e.SystemCode equals s.SystemCode
                             join r in _context.MRegisters on s.OwnerSystemCode equals r.OrganizationCode
                             select new TErrorApiLogModels
                             {
                                 Id = e.Id,
                                 Createdate = e.Createdate,
                                 CreatedBy = e.CreatedBy,
                                 SystemCode = e.SystemCode,
                                 Message = e.Message,
                                 StackTrace = e.StackTrace,
                                 ErrorDate = e.ErrorDate,
                                 UserName = e.UserName,
                                 Path = e.Path,
                                 HttpMethod = e.HttpMethod,
                                 RequestData = e.RequestData,
                                 InnerException = e.InnerException,
                                 Source = e.Source,
                                 SystemName = s.SystemName,
                                 ApiKey = r.ApiKey,
                                    HttpCode = e.HttpCode,
                             }).AsQueryable();

                // Apply Filters
                if (!string.IsNullOrEmpty(xmodel.SystemCode))
                {
                    query = query.Where(u => u.SystemCode == xmodel.SystemCode);
                }
                if (!string.IsNullOrEmpty(xmodel.SystemName))
                {
                    query = query.Where(u => u.SystemName.Contains(xmodel.SystemName));
                }
                if (xmodel?.Createdate != null)
                {
                    query = query.Where(u => u.Createdate.Value.Date == xmodel.Createdate.Value.Date);
                }
                xresult.totalList = query.Count();
                if (xmodel.rowFetch != 0)
                    query = query.Skip(xmodel.rowOFFSet).Take(xmodel.rowFetch);

                xresult.LError = await query.ToListAsync();
                return xresult;
            }
            catch
            {
                return new ViewErroApiModels();
            }
        }

        public async Task<TErrorApiLog?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.TErrorApiLogs.FindAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> AddAsync(TErrorApiLog log)
        {
            try
            {
                _context.TErrorApiLogs.Add(log);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateAsync(TErrorApiLog log)
        {
            try
            {
                _context.TErrorApiLogs.Update(log);
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
                var log = await _context.TErrorApiLogs.FindAsync(id);
                if (log == null) return 0;
                _context.TErrorApiLogs.Remove(log);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }
    }

    public interface ITErrorApiLogRepository
    {
        Task<ViewErroApiModels> GetAllAsync(TErrorApiLogModels xmodel);
        Task<TErrorApiLog?> GetByIdAsync(int id);
        Task<int> AddAsync(TErrorApiLog log);
        Task<int> UpdateAsync(TErrorApiLog log);
        Task<int> DeleteAsync(int id);
    }
}