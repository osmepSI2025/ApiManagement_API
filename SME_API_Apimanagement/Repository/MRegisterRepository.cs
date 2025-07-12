using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public class MRegisterRepository : IMRegisterRepository
    {
        private readonly ApiMangeDBContext _context;
        private readonly ITAPIMappingRepository _apiMappingRepository; // Use interface instead of concrete class

        // Update constructor to use ITAPIMappingRepository
        public MRegisterRepository(ApiMangeDBContext context, ITAPIMappingRepository apiMappingRepository)
        {
            _context = context;
            _apiMappingRepository = apiMappingRepository;
        }

        // 📌 ดึงข้อมูลทั้งหมด
        public async Task<IEnumerable<MRegister>> GetRegistersAsync()
        {
            return await _context.MRegisters.ToListAsync();
        }

        // 📌 ดึงข้อมูลตาม Id
        public async Task<MRegister> GetRegisterByIdAsync(string apikey)
        {
            try
            {
                return await _context.MRegisters
                    .FirstOrDefaultAsync(e => e.ApiKey == apikey);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<MRegister>> GetRegister(MRegisterModels xModels)
        {
            try
            {
                var query = _context.MRegisters.AsQueryable().Where(x => x.FlagDelete == "N");

                if (xModels?.Id != null && xModels.Id > 0 && !string.IsNullOrEmpty(xModels?.OrganizationCode))
                {
                    query = query.Where(u => u.Id == xModels.Id);
                   // query = query.Where(u => u.OrganizationCode == xModels.OrganizationCode);
                }
                if (!string.IsNullOrEmpty(xModels?.OrganizationCode) && (xModels?.Id != null && xModels.Id == 0))
                {
                    query = query.Where(u => u.OrganizationCode == xModels.OrganizationCode);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<MRegister>();
            }
        }

        public async Task<ViewRegisterApiModels> GetRegisterBySearch(MRegisterModels xModels)
        {
            var result = new ViewRegisterApiModels();
            try
            {
                var query = from r in _context.MRegisters
                            join o in _context.MOrganizations on r.OrganizationCode equals o.OrganizationCode
                            where r.FlagDelete == "N"
                            select new MRegisterModels
                            {
                                Id = r.Id,
                                OrganizationCode = r.OrganizationCode,
                                StartDate = r.StartDate,
                                EndDate = r.EndDate,
                                OrganizationName = o.OrganizationName,
                                FlagActive = r.FlagActive,
                                ApiKey = r.ApiKey,
                                CreateDate = r.CreateDate,
                                UpdateDate = r.UpdateDate,
                                Note = r.Note,
                            };

                if (!string.IsNullOrEmpty(xModels?.OrganizationName))
                {
                    query = query.Where(u => u.OrganizationName.Contains(xModels.OrganizationName));
                }
                if (!string.IsNullOrEmpty(xModels?.OrganizationCode))
                {
                    query = query.Where(u => u.OrganizationCode == xModels.OrganizationCode);
                }
                if (!string.IsNullOrEmpty(xModels?.ApiKey))
                {
                    query = query.Where(u => u.ApiKey.Contains(xModels.ApiKey));
                }
                if (xModels?.FlagActive != null)
                {
                    query = query.Where(u => u.FlagActive == xModels.FlagActive);
                }
                if (xModels?.CreateDate != null)
                {
                    query = query.Where(u => u.CreateDate.Value.Date == xModels.CreateDate.Value.Date);
                }
                if (xModels?.UpdateDate != null)
                {
                    query = query.Where(u => u.UpdateDate.Value.Date == xModels.UpdateDate.Value.Date);
                }
                if (xModels?.StartDate != null && xModels.EndDate != null)
                {
                    var start = xModels.StartDate.Value.Date;
                    var end = xModels.EndDate.Value.Date;

                    query = query.Where(u =>
                        u.StartDate.HasValue && u.EndDate.HasValue &&
                        u.StartDate.Value.Date <= end &&
                        u.EndDate.Value.Date >= start && u.EndDate <= end
                    );
                }

                result.TotalRowsList = await query.CountAsync();
                if (xModels.rowFetch != 0)
                    query = query.Skip<MRegisterModels>(xModels.rowOFFSet).Take(xModels.rowFetch);

                result.LRegis = await query.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new ViewRegisterApiModels();
            }
        }

        // 📌 เพิ่มข้อมูล/อัปเดตข้อมูล
        public async Task<string> UpdateOrInsertRegister(UpSertRegisterApiModels xModels)
        {
            string success = "";
            string apiKey = "";

            try
            {
                // Insert logic for new record
                if (xModels.MRegister.Id == 0)
                {
                    var xRaw = new MRegister
                    {
                        StartDate = xModels.MRegister.StartDate,
                        EndDate = xModels.MRegister.EndDate,
                        OrganizationCode = xModels.MRegister.OrganizationCode,
                        ApiKey = Guid.NewGuid().ToString(),
                        FlagActive = xModels.MRegister.FlagActive ?? true,
                        FlagDelete = "N",
                        Note = xModels.MRegister.Note,
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateBy = xModels.MRegister.CreateBy,
                        UpdateBy = xModels.MRegister.CreateBy
                    };

                    _context.MRegisters.Add(xRaw);
                    await _context.SaveChangesAsync();
                    success = xRaw.ApiKey;
                }
                else
                {
                    // Update logic for existing record
                    var result = await GetRegister(xModels.MRegister);

                  

                    if (result.Count > 0)
                    {
                      
                        foreach (var item in result)
                        {
                            if (item.OrganizationCode != xModels.MRegister.OrganizationCode)
                            {
                                var del = await _apiMappingRepository.DeleteByOrganizationCode(xModels.MRegister.OrganizationCode); // Delete existing mappings for this register
                            }
                            var queryUpdate = await _context.MRegisters
                                .FirstOrDefaultAsync(u => u.Id == item.Id);

                            if (queryUpdate != null)
                            {
                                apiKey = queryUpdate.ApiKey;

                                // Only update fields if provided
                                if (!string.IsNullOrEmpty(xModels.MRegister.OrganizationCode))
                                    queryUpdate.OrganizationCode = xModels.MRegister.OrganizationCode;
                                if (xModels.MRegister.StartDate != null)
                                    queryUpdate.StartDate = xModels.MRegister.StartDate;
                                if (xModels.MRegister.EndDate != null)
                                    queryUpdate.EndDate = xModels.MRegister.EndDate;
                                if (xModels.MRegister.FlagActive != null)
                                    queryUpdate.FlagActive = xModels.MRegister.FlagActive;
                                // Always set FlagDelete to "N" on update
                                queryUpdate.FlagDelete = "N";
                                if (!string.IsNullOrEmpty(xModels.MRegister.UpdateBy))
                                    queryUpdate.UpdateBy = xModels.MRegister.UpdateBy;
                                queryUpdate.Note = xModels.MRegister.Note;
                                queryUpdate.UpdateDate = DateTime.Now;
                            }
                        }

                        await _context.SaveChangesAsync();
                        success = apiKey;
                    }
                    else
                    {
                        // Fallback insert logic if no record found
                        var xRaw = new MRegister
                        {
                            StartDate = xModels.MRegister.StartDate,
                            EndDate = xModels.MRegister.EndDate,
                            OrganizationCode = xModels.MRegister.OrganizationCode,
                            ApiKey = Guid.NewGuid().ToString(),
                            FlagActive = xModels.MRegister.FlagActive ?? true,
                            FlagDelete = "N",
                            Note = xModels.MRegister.Note,
                            UpdateDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateBy = xModels.MRegister.CreateBy,
                            UpdateBy = xModels.MRegister.CreateBy
                        };

                        _context.MRegisters.Add(xRaw);
                        await _context.SaveChangesAsync();
                        success = xRaw.ApiKey;
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider logging ex for debugging
                success = "";
            }

            return success;
        }

        // 📌 อัปเดตข้อมูล
        public async Task UpdateRegisterAsync(MRegister register)
        {
            var existingRegister = await _context.MRegisters.FindAsync(register.Id);
            if (existingRegister != null)
            {
                existingRegister.OrganizationCode = register.OrganizationCode;
                existingRegister.StartDate = register.StartDate;
                existingRegister.EndDate = register.EndDate;
                existingRegister.FlagActive = register.FlagActive;
                existingRegister.FlagDelete = register.FlagDelete;
                existingRegister.UpdateBy = register.UpdateBy;
                existingRegister.UpdateDate = DateTime.Now;
                existingRegister.ApiKey = register.ApiKey;
                existingRegister.Note = register.Note;
                existingRegister.UpdateDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }

        // 📌 ลบข้อมูล
        public async Task<bool> DeleteRegisterAsync(int id)
        {
            try
            {
                var register = await _context.MRegisters.FindAsync(id);
                if (register != null)
                {
                    _context.MRegisters.Remove(register);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task UpdateStatus(MRegisterModels models)
        {
            var register = await _context.MRegisters.FindAsync(models.Id);
            if (register != null)
            {
                register.FlagActive = models.FlagActive;
                register.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
