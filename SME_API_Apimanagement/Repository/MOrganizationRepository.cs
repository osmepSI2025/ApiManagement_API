using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public class MOrganizationRepository : IMOrganizationRepository
    {
        private readonly ApiMangeDBContext _context;

        public MOrganizationRepository(ApiMangeDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MOrganization>> GetAllAsync() =>
            await _context.MOrganizations.Where(x => x.FlagDelete == "N").ToListAsync();

        public async Task<MOrganization> GetByIdAsync(int id) =>
       await _context.MOrganizations
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.OrganizationId == id && x.FlagDelete == "N");

        public async Task AddAsync(MOrganization organization)
        {
            _context.MOrganizations.Add(organization);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MOrganization organization)
        {
            _context.MOrganizations.Update(organization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var organization = await _context.MOrganizations.FindAsync(id);
            if (organization != null)
            {
                organization.FlagDelete = "Y";
                organization.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<MOrganizationModels>> GetOrgBySeach(MOrganizationModels xModels)
        {
            try
            {
                var query = from o in _context.MOrganizations
                            where o.FlagDelete == "N"
                            select new MOrganizationModels
                            {
                                OrganizationId = o.OrganizationId,
                                CreateBy = o.CreateBy,
                                CreateDate = o.CreateDate,
                                Address = o.Address,
                                City = o.City,
                                Country = o.Country,
                                Description = o.Description,
                                Email = o.Email,
                                FlagActive = o.FlagActive,
                                FlagDelete = o.FlagDelete,
                                IndustryType = o.IndustryType,
                                LogoUrl = o.LogoUrl,
                                OrganizationCode = o.OrganizationCode,
                                OrganizationName = o.OrganizationName,
                                ParentOrganizationId = o.ParentOrganizationId,
                                PhoneNumber = o.PhoneNumber,
                                PostalCode = o.PostalCode,
                                StateOrProvince = o.StateOrProvince,
                                Status = o.Status,
                                TaxId = o.TaxId,
                                UpdateBy = o.UpdateBy,
                                UpdateDate = o.UpdateDate,
                                WebsiteUrl = o.WebsiteUrl,

                            };

                if (!string.IsNullOrEmpty(xModels?.OrganizationCode))
                {
                    query = query.Where(u => u.OrganizationCode.Contains(xModels.OrganizationCode));
                }
                if (!string.IsNullOrEmpty(xModels?.OrganizationName))
                {
                    query = query.Where(u => u.OrganizationName.Contains( xModels.OrganizationName));
                }
                if (xModels?.CreateDate!=null)
                {
                    query = query.Where(u => u.CreateDate.Value.Date == xModels.CreateDate.Value.Date);
                }
                if (xModels?.UpdateDate != null)
                {
                    query = query.Where(u => u.UpdateDate.Value.Date == xModels.UpdateDate.Value.Date);
                }
                if (xModels.rowFetch != 0)
                    query = query.Skip<MOrganizationModels>(xModels.rowOFFSet).Take(xModels.rowFetch);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<MOrganizationModels>(); // Return List เปล่าแทน null
            }
        }

        public async Task<int> UpsertOrg(MOrganizationModels xModels)
        {
            try
            {
                var result = await GetByIdAsync(xModels.OrganizationId);

                if (result != null)
                {
                    // สร้าง model สำหรับ update
                    var xRaw = new MOrganization
                    {
                        OrganizationId = result.OrganizationId,
                        OrganizationName = xModels.OrganizationName,
                        OrganizationCode = xModels.OrganizationCode,
                        Description = xModels.Description,
                        FlagActive = xModels.FlagActive,
                        UpdateBy = xModels.CreateBy,
                        UpdateDate = DateTime.Now,
                        FlagDelete = "N",
                    };
                    try
                    {
                        await UpdateAsync(xRaw);
                        return result.OrganizationId;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }

                }
                else
                {
                    //var allItems = await GetAllAsync();
                    //var resultRuning = allItems.OrderByDescending(x => x.OrganizationId).FirstOrDefault();


                    var xRaw = new MOrganization
                    {
                       // OrganizationCode = xModels.OrganizationCode,
                       OrganizationCode = "EX_"+ Guid.NewGuid().ToString("N"),
                        OrganizationName = xModels.OrganizationName,
                        Email = xModels.Email,

                        Description = xModels.Description,
                        FlagActive = xModels.FlagActive,
                        FlagDelete = "N",
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateBy = xModels.CreateBy,
                        UpdateBy = xModels.CreateBy
                    };

                    _context.MOrganizations.Add(xRaw);
                    await _context.SaveChangesAsync();

                    return xRaw.OrganizationId;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }

}
