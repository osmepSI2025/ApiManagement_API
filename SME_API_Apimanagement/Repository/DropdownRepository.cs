using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Repository
{
    public class DropdownRepository : ControllerBase, IDropdownRepository
    {
        private readonly ApiMangeDBContext _context;
        public DropdownRepository(ApiMangeDBContext context)
        {
            _context = context;
        }


        public List<DropdownModels> GetDropdownLookUp(string LookUptype)
        {
            try
            {
                var query = (from u in _context.MLookups
                             where u.LookupType == LookUptype && u.FlagActive == "Y" && u.FlagDelete == "N"
                             select new DropdownModels
                             {
                                 Code = u.LookupCode,
                                 Name = u.LookupValue,
                             }
                             );
                return query.OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public List<DropdownModels> GetDropdownRole()
        //{
        //    try
        //    {
        //        var query = (from u in _context.MRoles
        //                     where u.FlagActive == "Y" && u.FlagDelete == "N"
        //                     select new DropdownModels
        //                     {
        //                         Code = u.RoleCode,
        //                         Name = u.RoleName,
        //                     }
        //                     );
        //        return query.OrderBy(x => x.Name).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public List<DropdownModels> GetDropdownSystem()
        {
            try
            {
                var query = (from u in _context.MSystems
                                     where u.FlagActive == true && u.FlagDelete == "N"
                             select new DropdownModels
                             {
                                 Code = u.SystemCode,
                                 Name = u.SystemName,
                             }
                             );
                return query.OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<DropdownModels> GetDropdownOrganization()
        {
            try
            {
                var query = (from u in _context.MOrganizations
                                     where u.FlagActive == true && u.FlagDelete == "N"
                             select new DropdownModels
                             {
                                 Code = u.OrganizationCode,
                                 Name = u.OrganizationName,
                             }
                             );
                return query.OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
            public List<DropdownModels> GetDropdownOrganizationWithOutData(List<MOrganizationModels> WoModels)
        {

            try
            {
                // Get the list of codes to exclude
                var excludeCodes = WoModels.Select(w => w.OrganizationCode).ToList();

                // Materialize the query after filtering in the database
                var query = (from u in _context.MOrganizations
                             where u.FlagActive == true && u.FlagDelete == "N"
                             select new DropdownModels
                             {
                                 Code = u.OrganizationCode,
                                 Name = u.OrganizationName,
                             }
                            ).ToList();

                // Now filter out the excluded codes in memory
                var result = query.Where(x => !excludeCodes.Contains(x.Code)).OrderBy(x => x.Name).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
