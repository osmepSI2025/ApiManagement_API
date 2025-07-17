using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Service;

namespace SME_API_Apimanagement.Repository
{
    public class MSystemRepository : IMSystemRepository
    {
        private readonly ApiMangeDBContext _context;
        private readonly HrEmployeeService _hrEmployee;
        private readonly IMOrganizationRepository _org;
        public MSystemRepository(ApiMangeDBContext context, HrEmployeeService hrEmployee, IMOrganizationRepository org)
        {
            _context = context;
            _hrEmployee = hrEmployee;
            _org = org;
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
            if (model.StartDate!=null) 
            {
                entity.StartDate = model.StartDate;
            }
            if (model.EndDate != null)
            {
                entity.EndDate = model.EndDate;
            }
            if (model.SystemName != null)
            {
                entity.SystemName = model.SystemName;
            }
            if (model.FlagActive != null)
            {
                entity.FlagActive = model.FlagActive;
            }
            if (model.OwnerSystemCode != null)
            {
                entity.OwnerSystemCode = model.OwnerSystemCode;
            }

            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = model.UpdateBy;
 
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var system = await _context.MSystems.FindAsync(id);

                //delete sysinfo too

                try {
                    var sysInfos = await _context.MSystemInfos
        .Where(x => x.SystemCode == system.SystemCode)
        .ToListAsync();

                    if (sysInfos.Any())
                    {
                        _context.MSystemInfos.RemoveRange(sysInfos);
                        await _context.SaveChangesAsync();
                    }
                } catch (Exception ex)
                {
                
                }

                //delete api sys mapp
                try
                {
                    var sysApiMapping = await _context.TSystemApiMappings
       .Where(x => x.OwnerSystemCode == system.SystemCode)
       .ToListAsync();
                    if (sysApiMapping.Any())
                    {
                        _context.TSystemApiMappings.RemoveRange(sysApiMapping);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {

                }

                //delete api permission mapping
                try {
                    var sysApiPermissionMapping = await _context.TApiPermisionMappings
       .Where(x => x.SystemCode == system.SystemCode)
       .ToListAsync();
                    if (sysApiPermissionMapping.Any())
                    {
                        _context.TApiPermisionMappings.RemoveRange(sysApiPermissionMapping);
                        await _context.SaveChangesAsync();
                    }
                } catch (Exception ex)
                {
                
                }
            


                if (system != null)
                {
                    system.FlagDelete = "Y";
                    system.UpdateDate = DateTime.UtcNow;
                    var xint = await _context.SaveChangesAsync();
                    return xint; // Return the number of rows updated
                }
                return 1; // Return 0 if the system is not found
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
                if (xModels.Id !=0) 
                {
                    var result = await GetByIdAsync(xModels.Id);

                    if (result != null)
                    {
                        // สร้าง model สำหรับ update
                        var xRaw = new MSystem
                        {
                            Id = result.Id,
                            SystemName = xModels.SystemName,
                            FlagActive = xModels.FlagActive ?? false,
                            UpdateBy = xModels.UpdateBy,
                            OwnerSystemCode = xModels.OwnerSystemCode,
                            StartDate = xModels.StartDate,
                            EndDate = xModels.EndDate,

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
                            FlagActive = xModels.FlagActive ?? false,
                            FlagDelete = "N",
                            UpdateDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateBy = xModels.CreateBy,
                            UpdateBy = xModels.CreateBy,
                            OwnerSystemCode = xModels.OwnerSystemCode,
                            StartDate = xModels.StartDate,
                            EndDate = xModels.EndDate,
                        };

                        _context.MSystems.Add(xRaw);
                        await _context.SaveChangesAsync();

                        return xRaw.Id;
                    }
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
                        FlagActive = xModels.FlagActive ?? false,
                        FlagDelete = "N",
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateBy = xModels.CreateBy,
                        UpdateBy = xModels.CreateBy,
                        OwnerSystemCode = xModels.OwnerSystemCode,
                        StartDate = xModels.StartDate,
                        EndDate = xModels.EndDate,
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
            var Ldepart = await _org.GetAllAsync();
           
            var dpart = Ldepart.ToList();

            try
            {
                if (xModels.EmployeeRole == "SUPERADMIN")
                {
                    // 1. ดึงข้อมูลระบบจากฐานข้อมูล
                    var mSystems = await _context.MSystems
                        .Where(s => s.FlagDelete == "N")
                        .ToListAsync();

                    // 2. Join กับ dpart ใน memory
                    var query = from s in mSystems
                                join dp in dpart on s.OwnerSystemCode equals dp.OrganizationCode into dpJoin
                                from dp in dpJoin.DefaultIfEmpty()
                                select new MSystemModels
                                {
                                    Id = s.Id,
                                    CreateBy = s.CreateBy,
                                    FlagDelete = s.FlagDelete,
                                    UpdateBy = s.UpdateBy,
                                    FlagActive = s.FlagActive ?? false,
                                    IsSelected = s.FlagActive ?? false,
                                    SystemCode = s.SystemCode,
                                    SystemName = s.SystemName,
                                    CreateDate = s.CreateDate,
                                    UpdateDate = s.UpdateDate,
                                    OwnerSystemCode = s.OwnerSystemCode,
                                    OwnerSystemName = dp?.OrganizationName,
                                    StartDate = s.StartDate,
                                    EndDate = s.EndDate
                                };

                    // Apply Filters (ใน memory)
                    if (!string.IsNullOrEmpty(xModels?.SystemName))
                        query = query.Where(u => u.SystemName != null && u.SystemName.Contains(xModels.SystemName));
                    if (xModels?.CreateDate != null)
                        query = query.Where(u => u.CreateDate.HasValue && u.CreateDate.Value.Date == xModels.CreateDate.Value.Date);
                    if (!string.IsNullOrEmpty(xModels?.FlagDelete))
                        query = query.Where(u => u.FlagDelete == xModels.FlagDelete);
                    if (xModels?.FlagActive != null)
                        query = query.Where(u => u.FlagActive == xModels.FlagActive);
                    if (xModels?.StartDate != null && xModels?.EndDate != null)
                    {
                        var start = xModels.StartDate.Value.Date;
                        var end = xModels.EndDate.Value.Date;
                        query = query.Where(u =>
                              u.StartDate.HasValue && u.EndDate.HasValue &&
                            u.StartDate.Value.Date >= start &&
                            u.StartDate.Value.Date <= end &&
                            u.EndDate.Value.Date >= start
                            && u.EndDate.Value.Date <= end
                        );
                    }
                    if (xModels.rowFetch != 0)
                        query = query.Skip(xModels.rowOFFSet).Take(xModels.rowFetch);

                    return query.ToList();
                }
                else
                {
                    // User ทั่วไป เห็นเฉพาะระบบที่ mapping กับตนเอง
                    var empmapsys = await _context.TEmployeeMapSystems
                        .Where(em => em.EmployeeId == xModels.EmployeeId)
                        .Select(em => new { em.EmployeeId, em.SystemApiId })
                        .ToListAsync();

                    var mSystems = await _context.MSystems
                        .Where(s => s.FlagDelete == "N")
                        .ToListAsync();

                    var query = from s in mSystems
                                join em in empmapsys on s.Id equals em.SystemApiId
                                join dp in dpart on s.OwnerSystemCode equals dp.OrganizationCode into dpJoin
                                from dp in dpJoin.DefaultIfEmpty()
                                select new MSystemModels
                                {
                                    Id = s.Id,
                                    CreateBy = s.CreateBy,
                                    FlagDelete = s.FlagDelete,
                                    UpdateBy = s.UpdateBy,
                                    FlagActive = s.FlagActive?? false,
                                    IsSelected = s.FlagActive ?? false,
                                    SystemCode = s.SystemCode,
                                    SystemName = s.SystemName,
                                    CreateDate = s.CreateDate,
                                    UpdateDate = s.UpdateDate,
                                    OwnerSystemCode = s.OwnerSystemCode,
                                    OwnerSystemName = dp?.OrganizationName,
                                    StartDate = s.StartDate,
                                    EndDate = s.EndDate
                                };

                    // Apply Filters (ใน memory)
                    if (!string.IsNullOrEmpty(xModels?.SystemName))
                        query = query.Where(u => u.SystemName != null && u.SystemName.Contains(xModels.SystemName));
                    if (xModels?.CreateDate != null)
                        query = query.Where(u => u.CreateDate.HasValue && u.CreateDate.Value.Date == xModels.CreateDate.Value.Date);
                    if (!string.IsNullOrEmpty(xModels?.FlagDelete))
                        query = query.Where(u => u.FlagDelete == xModels.FlagDelete);
                    if (xModels?.FlagActive != null)
                        query = query.Where(u => u.FlagActive == xModels.FlagActive);
                    if (xModels?.StartDate != null && xModels?.EndDate != null)
                    {
                        var start = xModels.StartDate.Value.Date;
                        var end = xModels.EndDate.Value.Date;
                        query = query.Where(u =>
                            u.StartDate.HasValue && u.EndDate.HasValue &&
                            u.StartDate.Value.Date >= start &&
                            u.StartDate.Value.Date <= end &&
                            u.EndDate.Value.Date >= start
                            && u.EndDate.Value.Date <= end
                        );
                    }
                    if (xModels.rowFetch != 0)
                        query = query.Skip(xModels.rowOFFSet).Take(xModels.rowFetch);

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                return new List<MSystemModels>();
            }
        }

        public async Task<List<MSystemModels>> GetSystemBySearchMaster(MSystemModels xModels)
        {
            try
            {

                var querySuperAdmin = (from s in _context.MSystems
                                       where s.FlagDelete == "N"
                                       select new MSystemModels
                                       {
                                           Id = s.Id,
                                           CreateBy = s.CreateBy,
                                           FlagDelete = s.FlagDelete,
                                           UpdateBy = s.UpdateBy,
                                           FlagActive = s.FlagActive?? false,
                                           IsSelected = s.FlagActive ?? false,
                                           SystemCode = s.SystemCode,
                                           SystemName = s.SystemName,
                                           CreateDate = s.CreateDate,
                                           UpdateDate = s.UpdateDate,
                                           OwnerSystemCode = s.OwnerSystemCode
                                           ,
                                           StartDate = s.StartDate
                                        ,
                                           EndDate = s.EndDate

                                       }).AsQueryable(); // ทำให้ Query เป็น IQueryable

                // Apply Filters
                if (!string.IsNullOrEmpty(xModels?.SystemName))
                {
                    querySuperAdmin = querySuperAdmin.Where(u => u.SystemName.Contains(xModels.SystemName));
                }

                if (xModels?.CreateDate != null)
                {
                    querySuperAdmin = querySuperAdmin.Where(u => u.CreateDate.Value.Date == xModels.CreateDate.Value.Date);
                }

                if (!string.IsNullOrEmpty(xModels?.SystemCode))
                {
                    querySuperAdmin = querySuperAdmin.Where(u => u.SystemCode==xModels.SystemCode);
                }
                if (xModels.EmployeeRole != "SUPERADMIN")
                {
                    //if (xModels?.CreateBy != null)
                    //{
                    //    querySuperAdmin = querySuperAdmin.Where(u => u.CreateBy == xModels.CreateBy);
                    //}

                }

               

                if (xModels?.StartDate != null && xModels?.EndDate != null)
                {
                    var start = xModels.StartDate.Value.Date;
                    var end = xModels.EndDate.Value.Date;

                    querySuperAdmin = querySuperAdmin.Where(u =>
                        u.StartDate.HasValue && u.EndDate.HasValue &&
                        u.StartDate.Value.Date >= start &&
                        u.StartDate.Value.Date <= end &&
                        u.EndDate.Value.Date >= start &&
                            u.EndDate.Value.Date <= end
                    );
                }

                if (xModels.FlagSearch == "SEARCH")
                {
                
                }
                else 
                {
                    if (xModels?.FlagActive != null)

                    {
                        querySuperAdmin = querySuperAdmin.Where(u => u.FlagActive == xModels.FlagActive);
                    }
                }
                if (xModels.rowFetch != 0)
                    querySuperAdmin = querySuperAdmin.Skip<MSystemModels>(xModels.rowOFFSet).Take(xModels.rowFetch);
                return querySuperAdmin.ToList();

            }
            catch (Exception ex)
            {
                return new List<MSystemModels>(); // Return List เปล่าแทน null
            }
        }

        public async Task<bool> UpdateStatus(MSystemModels models)
        {
            try {
                var register = await _context.MSystems.FindAsync(models.Id);
                if (register != null)
                {
                    register.FlagActive = models.FlagActive;
                    register.UpdateDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                // Map 'register' (MSystem) to 'MSystemModels' before passing it to UpdateStatusByCode
                var mappedModel = new MSystemModels
                {
                    Id = register.Id,
                    SystemCode = register.SystemCode,
                 
                    FlagActive = models.FlagActive,
                    FlagDelete = register.FlagDelete,
            
                };

                await UpdateStatusByCode(mappedModel);
                return true;
            } catch (Exception ex) 
            
            {
                return false;
            }
          
        }
        public async Task<bool> UpdateStatusByCode(MSystemModels models)
        {
            try
            {
                var register = await _context.MSystems
                    .Where(x => x.SystemCode == models.SystemCode)
                    .FirstOrDefaultAsync();

                // Update all related MSystemInfo records
                try
                {
                    var sysInfos = await _context.MSystemInfos
                        .Where(x => x.SystemCode == models.SystemCode)
                        .ToListAsync();

                    if (sysInfos.Any())
                    {
                        foreach (var sysInfo in sysInfos)
                        {
                            sysInfo.FlagActive = models.FlagActive;
                            sysInfo.UpdateDate = DateTime.UtcNow;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex) { }

                if (register != null)
                {
                    register.FlagActive = models.FlagActive;
                    register.UpdateDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
