using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public class TAPIMappingRepository : ITAPIMappingRepository
    {
        private readonly ApiMangeDBContext _context;
        private readonly IErrorApiLogRepository _errorApiLogRepository;
        public TAPIMappingRepository(ApiMangeDBContext context, IErrorApiLogRepository errorApiLogRepository)
        {
            _context = context;
            _errorApiLogRepository = errorApiLogRepository;
        }

        public async Task<IEnumerable<TApiPermisionMapping>> GetAllAsync() =>
            await _context.TApiPermisionMappings.Where(x => x.FlagDelete == "N").ToListAsync();

        public async Task<TApiPermisionMapping> GetByIdAsync(int id) =>
            await _context.TApiPermisionMappings.FirstOrDefaultAsync(x => x.Id == id && x.FlagDelete == "N");

        public async Task AddAsync(TApiPermisionMapping mapping)
        {
            _context.TApiPermisionMappings.Add(mapping);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TApiPermisionMapping mapping)
        {
            _context.TApiPermisionMappings.Update(mapping);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mapping = await _context.TApiPermisionMappings.FindAsync(id);
            if (mapping != null)
            {
                mapping.FlagDelete = "Y";
                mapping.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> DeleteByOrganizationCode(string orgCode)
        {
            var records = _context.TApiPermisionMappings.Where(x => x.OrganizationCode == orgCode);

            if (records.Any())
            {
                _context.TApiPermisionMappings.RemoveRange(records);
                return await _context.SaveChangesAsync();
            }
            else
            {
                return 1;
            }


        }

        public async Task<int> DeleteByOrganizationCodeAndRegister(string org, int registerId)
        {
            var records = _context.TApiPermisionMappings.Where(x =>  x.RegisterId == registerId && x.OrganizationCode == org);

            if (records.Any())
            {
                _context.TApiPermisionMappings.RemoveRange(records);
                return await _context.SaveChangesAsync();
            }
            else
            {
                return 1;
            }


        }
        public async Task<int> UpdateOrInsertTApiMapping(UpSertRegisterApiModels xModels, string apiKey)
        {
            int success = 0;

            try
            {
                var result = await DeleteByOrganizationCodeAndRegister(xModels.MRegister.OrganizationCode,xModels.MRegister.Id); // เรียกใช้ await

                if (result != 0)
                {
                    foreach (var item in xModels.LPerMapApi)
                    {
                        var xRaw = new TApiPermisionMapping
                        {
                            StartDate = xModels.MRegister.StartDate,
                            EndDate = xModels.MRegister.EndDate,
                            OrganizationCode = item.OrganizationCode,
                            SystemCode = item.SystemCode,
                            ApiKey = apiKey,
                            FlagActive = item.IsSelected,
                            FlagDelete = "N",
                            UpdateDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateBy = xModels.MRegister.CreateBy,
                            UpdateBy = xModels.MRegister.UpdateBy,
                            SystemApiMappingId = item.SystemApiMappingId,
                            RegisterId = xModels.MRegister.Id,


                        };

                        _context.TApiPermisionMappings.Add(xRaw);
                        await _context.SaveChangesAsync();
                        success = xRaw.Id; // ดึงค่า Id หลัง Save
                    }

                    success = await _context.SaveChangesAsync(); // เรียก SaveChangesAsync ครั้งเดียว
                    success = 1;
                }

            }
            catch (Exception ex)
            {
                success = 0; // ถ้า Error ให้ return 0
            }

            return success;
        }
        public async Task<List<TApiPermisionMappingModels>> GetTApiMappingBySearch(TApiPermisionMappingModels xModels)
        {
            var result = new List<TApiPermisionMappingModels>();
            try
            {
                var query = (
                  from ts in _context.TSystemApiMappings
                  join ms in _context.MSystems
                      on ts.OwnerSystemCode equals ms.SystemCode
                  join ta in _context.TApiPermisionMappings
                      on ts.Id equals ta.SystemApiMappingId into taGroup
                  from ta in taGroup.DefaultIfEmpty()
                  select new TApiPermisionMappingModels
                             {
                                    SystemCode = ms.SystemCode,
                                    SystemName = ms.SystemName,
                                  
                                    ApiKey = ta.ApiKey,
                                    FlagActive = ta.FlagActive,
                                    StartDate =ta.StartDate,
                                     EndDate = ta.EndDate,
                                     OrganizationCode = ta.OrganizationCode
                                     ,ServiceName = ts.ApiServiceName,
                      SystemApiMappingId = ts.Id,
                      IsSelected = ta.FlagActive?? false,
                     
                      

                  }).AsQueryable(); // ทำให้ Query เป็น IQueryable

                // Apply Filters
                if (!string.IsNullOrEmpty(xModels?.OrganizationCode))
                {
                    query = query.Where(u => u.OrganizationCode == xModels.OrganizationCode);
                }

                if (!string.IsNullOrEmpty(xModels?.ApiKey))
                {
                    query = query.Where(u => u.ApiKey == xModels.ApiKey);
                }

                if (query.ToList().Count>0)
                {
                    //check all api
                    var queryCheckdata = (
                 from ts in _context.TSystemApiMappings
                 join ms in _context.MSystems
                     on ts.OwnerSystemCode equals ms.SystemCode

                 select new TApiPermisionMappingModels
                 {
                     SystemCode = ms.SystemCode,
                     SystemName = ms.SystemName,
                     ServiceName = ts.ApiServiceName,
                     SystemApiMappingId = ts.Id,


                 }).AsQueryable(); // ทำให้ Query เป็น IQueryable
                    if (queryCheckdata.ToList().Count == query.ToList().Count)
                    {
                        result = await query.ToListAsync();
                    }
                    else 
                    {
                        var bDict = query.ToList()
                            .GroupBy(x => (x.SystemCode, x.SystemApiMappingId))
                            .ToDictionary(
                                g => g.Key,
                                g => g.First().FlagActive // or use your preferred logic for duplicates
                            );

                        result = queryCheckdata.ToList().Select(a =>
                        {
                            var key = (a.SystemCode, a.SystemApiMappingId);
                            var isSelected = bDict.TryGetValue(key, out bool? flagActive) ? flagActive : false;

                            return new TApiPermisionMappingModels
                            {
                                Id = a.Id,
                                SystemCode = a.SystemCode,
                                SystemName = a.SystemName,
                                SystemApiMappingId = a.SystemApiMappingId,
                                ServiceName = a.ServiceName,
                                OrganizationCode = a.OrganizationCode,
                                ApiKey = a.ApiKey,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                FlagActive = isSelected,
                                IsSelected = isSelected ?? false, // Use FlagActive as IsSelected
                            };
                        }).ToList();
                    }
                   
                }
                else 
                {
                    var queryNodata = (
                 from ts in _context.TSystemApiMappings
                 join ms in _context.MSystems
                     on ts.OwnerSystemCode equals ms.SystemCode
            
                 select new TApiPermisionMappingModels
                 {
                     SystemCode = ms.SystemCode,
                     SystemName = ms.SystemName,
                     ServiceName = ts.ApiServiceName,
                     SystemApiMappingId = ts.Id,
                     IsSelected =false,

                 }).AsQueryable(); // ทำให้ Query เป็น IQueryable
                    result = await queryNodata.ToListAsync();
                   
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<TApiPermisionMappingModels>(); // Return List เปล่าแทน null
            }
        }

        public async Task<List<TApiPermisionMappingModels>> GetAllByBusinessIdAsync(searchApiPermisionRespone xModels)
        {
          
                var query = (
               
                 //from ms in _context.MSystems                    
          
                 //join  o in _context.MOrganizations on ms.OwnerSystemCode equals o.OrganizationCode
                 //join ts in _context.TSystemApiMappings on ms.SystemCode equals ts.OwnerSystemCode
                 
                    from ts in _context.TApiPermisionMappings                  
                        join o in _context.MOrganizations on ts.OrganizationCode equals o.OrganizationCode
                        join tms in _context.TSystemApiMappings on ts.SystemApiMappingId equals tms.Id
                        join ms in _context.MSystems on tms.OwnerSystemCode equals ms.SystemCode
                   


                    select new TApiPermisionMappingModels
                 {
                     OrganizationCode = o.OrganizationCode,
                     OrganizationName =o.OrganizationName,
                     SystemCode = ms.SystemCode,
                     SystemName = ms.SystemName,                  
                     FlagActive = ts.FlagActive,    
                     ServiceName = tms.ApiServiceName,
                        ApiMethod = tms.ApiMethod,
                     ApiUrlUatOundbound = tms.ApiUrlUatOutbound,
                     ApiUrlProdOundbound = tms.ApiUrlProdInbound,
                     ApiKey = ts.ApiKey,
                 })  
                 ; // ทำให้ Query เป็น IQueryable
                   // Apply Filters
            if (!string.IsNullOrEmpty(xModels?.System_Code))
            {
                query = query.Where(u => u.OrganizationCode == xModels.System_Code);
            }
            if (!string.IsNullOrEmpty(xModels?.API_Key))
            {
                query = query.Where(u => u.ApiKey == xModels.API_Key);
            }
            if (!string.IsNullOrEmpty(xModels?.System_Name))
            {
                query = query.Where(u => u.SystemName.Contains(xModels.System_Name));
            }

            if (xModels?.FlagActive!=null)
            {
                query = query.Where(u => u.FlagActive== xModels.FlagActive);
            }
            return query.ToList();
           
        }

        public async Task<List<TApiPermisionMappingModels>> GetCheckTApiMappingBySearch(TApiPermisionMappingModels xModels)
        {
            var result = new List<TApiPermisionMappingModels>();
            try
            {
                var query = (
                  from ts in _context.TSystemApiMappings
                  join ms in _context.MSystems
                      on ts.OwnerSystemCode equals ms.SystemCode
                  join ta in _context.TApiPermisionMappings
                      on ts.Id equals ta.SystemApiMappingId
                  where ts.ApiServiceCode != null
                  select new TApiPermisionMappingModels
                  {
                      SystemCode = ms.SystemCode,
                      SystemName = ms.SystemName,

                      ApiKey = ta.ApiKey,
                      FlagActive = ta.FlagActive,
                      StartDate = ta.StartDate,
                      EndDate = ta.EndDate,
                      OrganizationCode = ta.OrganizationCode
                                     ,
                      ServiceName = ts.ApiServiceName,
                      SystemApiMappingId = ts.Id,
                      IsSelected = ta.FlagActive ?? false,
                      ApiServiceCode  = ts.ApiServiceCode,
                     

                  }).AsQueryable(); // ทำให้ Query เป็น IQueryable

                // Apply Filters
                if (!string.IsNullOrEmpty(xModels?.OrganizationCode))
                {
                    query = query.Where(u => u.OrganizationCode == xModels.OrganizationCode);
                }

                if (!string.IsNullOrEmpty(xModels?.ApiKey))
                {
                    query = query.Where(u => u.ApiKey == xModels.ApiKey);
                }

       
                return query.ToList();
            }
            catch (Exception ex)
            {
                return new List<TApiPermisionMappingModels>(); // Return List เปล่าแทน null
            }
        }
    }

}
