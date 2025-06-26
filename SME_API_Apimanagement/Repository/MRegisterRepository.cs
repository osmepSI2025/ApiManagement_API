using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using System;
using System.Linq;

namespace SME_API_Apimanagement.Repository
{
    public class MRegisterRepository : IMRegisterRepository
    {
        private readonly ApiMangeDBContext _context;

        public MRegisterRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        // 📌 ดึงข้อมูลทั้งหมด
        public async Task<IEnumerable<MRegister>> GetRegistersAsync()
        {
            return await _context.MRegisters.ToListAsync();
        }

        // 📌 ดึงข้อมูลตาม Id
        public async Task<MRegister> GetRegisterByIdAsync(string apikey)
        {
            //return await _context.MRegisters.FindAsync(id);
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
                var query = _context.MRegisters.AsQueryable(); // ใช้ IQueryable ให้ทำงานที่ Server-side

                if (!string.IsNullOrEmpty(xModels?.OrganizationCode))
                {
                    query = query.Where(u => u.OrganizationCode == xModels.OrganizationCode);
                }

                return await query.ToListAsync(); // ใช้ await กับ ToListAsync() เพื่อรองรับ async
            }
            catch (Exception ex)
            {
                return new List<MRegister>(); // ควร return List เปล่าแทน null
            }
        }
        public async Task<ViewRegisterApiModels> GetRegisterBySearch(MRegisterModels xModels)
        { 
            var result = new ViewRegisterApiModels();
            try
            {
                var query = from r in _context.MRegisters
                            join o in _context.MOrganizations on r.OrganizationCode equals o.OrganizationCode
                            select new MRegisterModels
                            {
                                Id = r.Id,
                                OrganizationCode = r.OrganizationCode,
                                StartDate = r.StartDate,
                                EndDate = r.EndDate,
                                OrganizationName = o.OrganizationName, // สมมติว่ามีฟิลด์ OrganizationName
                                FlagActive = r.FlagActive
                                ,ApiKey = r.ApiKey,
                                CreateDate = r.CreateDate,
                                UpdateDate = r.UpdateDate,

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
                result .TotalRowsList = await query.CountAsync(); // นับจำนวนแถวทั้งหมด
                if (xModels.rowFetch != 0)
                    query = query.Skip<MRegisterModels>(xModels.rowOFFSet).Take(xModels.rowFetch);

                result.LRegis = await query.ToListAsync(); // ดึงข้อมูลตามเงื่อนไขที่กำหนด
                return result;
            }
            catch (Exception ex)
            {
                return new ViewRegisterApiModels(); // Return List เปล่าแทน null
            }
        }
        // 📌 เพิ่มข้อมูล
        public async Task<string> UpdateOrInsertRegister(UpSertRegisterApiModels xModels)
        {
            string success = "";
            string apiKey = "";

            try
            {
                var result = await GetRegister(xModels.MRegister); // เรียกใช้ await

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var queryUpdate = await _context.MRegisters
                            .FirstOrDefaultAsync(u => u.Id == item.Id); // ใช้ FirstOrDefaultAsync
                        apiKey = queryUpdate.ApiKey;
                        if (queryUpdate != null)
                        {
                            if (xModels.MRegister.StartDate!=null)
                            queryUpdate.StartDate = xModels.MRegister.StartDate;
                            if (xModels.MRegister.EndDate != null)
                                queryUpdate.EndDate = xModels.MRegister.EndDate;
                            if (xModels.MRegister.FlagActive != null)
                                queryUpdate.FlagActive = xModels.MRegister.FlagActive;
                            if (xModels.MRegister.FlagDelete != null)
                                queryUpdate.FlagDelete = "N";


                            if (!string.IsNullOrEmpty(xModels.MRegister.UpdateBy))
                            {
                                queryUpdate.UpdateBy = xModels.MRegister.UpdateBy;
                            }

                            queryUpdate.UpdateDate = DateTime.Now;
                        }
                    }

                   await _context.SaveChangesAsync(); // เรียก SaveChangesAsync ครั้งเดียว
                    success = apiKey;
                }
                else
                {
                    var xRaw = new MRegister
                    {
                        StartDate = xModels.MRegister.StartDate,
                        EndDate = xModels.MRegister.EndDate,
                        OrganizationCode = xModels.MRegister.OrganizationCode,
                        ApiKey = Guid.NewGuid().ToString(),
                        FlagActive = true,
                        FlagDelete = "N",

                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateBy = xModels.MRegister.CreateBy,
                        UpdateBy = xModels.MRegister.CreateBy
                    };

                    _context.MRegisters.Add(xRaw);
                    await _context.SaveChangesAsync();
                    success = xRaw.ApiKey; // ดึงค่า Id หลัง Save
                
                }
            }
            catch (Exception ex)
            {
                success = ""; // ถ้า Error ให้ return 0
            }

            return success;
        }

        //public async Task AddRegisterAsync(MRegister register)
        //{
        //    register.CreateDate = DateTime.Now;
        //    _context.MRegisters.Add(register);
        //    await _context.SaveChangesAsync();
        //}

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
                await _context.SaveChangesAsync();
            }
        }

        // 📌 ลบข้อมูล
        public async Task DeleteRegisterAsync(int id)
        {
            var register = await _context.MRegisters.FindAsync(id);
            if (register != null)
            {
                register.FlagDelete = "Y";
                register.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
       
    }

}
