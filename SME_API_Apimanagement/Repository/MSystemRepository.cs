using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using System;

namespace SME_API_Apimanagement.Repository
{
    public class MSystemRepository : IMSystemRepository
    {
        private readonly ApiMangeDBContext _context;

        public MSystemRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MSystem>> GetAllAsync() =>
            await _context.MSystems.Where(x => x.FlagDelete == "N").ToListAsync();

        public async Task<MSystem> GetByIdAsync(int id) =>
           await _context.MSystems
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id && x.FlagDelete == "N");

        public async Task AddAsync(MSystem system)
        {
            _context.MSystems.Add(system);
            await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(MSystem model)
        {
            var entity = await _context.MSystems.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null) return 0;

            // อัปเดตเฉพาะ field ที่ต้องการ
            entity.SystemName = model.SystemName;
            entity.FlagActive = model.FlagActive;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = model.UpdateBy;
            entity.OwnerSystemCode = model.OwnerSystemCode;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var system = await _context.MSystems.FindAsync(id);
                if (system != null)
                {
                    system.FlagDelete = "Y";
                    system.UpdateDate = DateTime.UtcNow;
                    var xint = await _context.SaveChangesAsync();
                    return xint; // Return the number of rows updated
                }
                return 0; // Return 0 if the system is not found
            }
            catch (Exception ex)
            {
                // Handle the error here, e.g., log the error
                return 0; // Return 0 if an error occurs
            }
        }

        public async Task<int> UpsertSystem(MSystemModels xModels)
        {
            try
            {
                var result = await GetByIdAsync(xModels.Id);

                if (result != null)
                {
                    // สร้าง model สำหรับ update
                    var xRaw = new MSystem
                    {
                        Id = result.Id,
                        SystemName = xModels.SystemName,
                        FlagActive = xModels.FlagActive,
                        UpdateBy = xModels.CreateBy,
                        OwnerSystemCode = xModels.OwnerSystemCode
                    };

                    await UpdateAsync(xRaw);
                    return result.Id;
                }
                else
                {
                    var allItems = await GetAllAsync();
                    var resultRuning = allItems.OrderByDescending(x => x.Id).FirstOrDefault();
                    var nextRunning = (resultRuning?.Runing ?? 0) + 1;

                    var xRaw = new MSystem
                    {
                        SystemCode = "SYS-" + nextRunning.ToString("D4"),
                        SystemName = xModels.SystemName,
                        Runing = nextRunning,
                        FlagActive = true,
                        FlagDelete = "N",
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateBy = xModels.CreateBy,
                        UpdateBy = xModels.CreateBy,
                        OwnerSystemCode = xModels.OwnerSystemCode
                    };

                    _context.MSystems.Add(xRaw);
                    await _context.SaveChangesAsync();

                    return xRaw.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<MSystemModels>> GetSystemBySearch(MSystemModels xModels)
        {
            try
            {
                var query = (from s in _context.MSystems
                             where s.FlagDelete == "N"
                             select new MSystemModels
                             {
                                 Id = s.Id,
                                 CreateBy = s.CreateBy,
                                 FlagDelete = s.FlagDelete,
                                 UpdateBy = s.UpdateBy,
                                 FlagActive = s.FlagActive,
                                 IsSelected = s.FlagActive?? false,
                                 SystemCode = s.SystemCode,
                                 SystemName = s.SystemName,
                                 CreateDate = s.CreateDate,
                                 UpdateDate = s.UpdateDate,
                                 OwnerSystemCode = s.OwnerSystemCode
                             }).AsQueryable(); // ทำให้ Query เป็น IQueryable

                // Apply Filters
                if (!string.IsNullOrEmpty(xModels?.SystemName))
                {
                    query = query.Where(u => u.SystemName.Contains(xModels.SystemName));
                }
                //if (xModels?.FlagActive != null)
                //{
                //    query = query.Where(u => u.FlagActive == xModels.FlagActive);
                //}
                if (xModels?.CreateDate != null)
                {
                    query = query.Where(u => u.CreateDate.Value.Date == xModels.CreateDate.Value.Date);
                }

                if (xModels.rowFetch != 0)
                    query = query.Skip<MSystemModels>(xModels.rowOFFSet).Take(xModels.rowFetch);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<MSystemModels>(); // Return List เปล่าแทน null
            }
        }
    }
}
