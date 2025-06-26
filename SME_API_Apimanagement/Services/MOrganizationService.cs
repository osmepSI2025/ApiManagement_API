using Microsoft.EntityFrameworkCore;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Service;
using System.Threading.Tasks;

namespace SME_API_Apimanagement.Services
{
    public class MOrganizationService
    {
        private readonly ApiMangeDBContext _context;
        private readonly HrEmployeeService _hrEmployee;

        public MOrganizationService(ApiMangeDBContext context,
             HrEmployeeService hrEmployee)
        {
            _context = context;
             _hrEmployee = hrEmployee;
        }

        public async Task UpsertAsync()
        {
            //call the API to get the organization data HR
            var Ldepart = _hrEmployee.GetDepartment();

            if (Ldepart.Result == null || Ldepart.Result.Results.Count()==0)
                return;
            foreach (var org in Ldepart.Result.Results)
            {
                var existing = await _context.MOrganizations
            .FirstOrDefaultAsync(x => x.OrganizationCode == org.BusinessUnitId);


                // If the organization does not exist, add it; otherwise, update it
                MOrganization mOrganization = new MOrganization
                {
                    OrganizationId = existing.OrganizationId,
                    OrganizationCode = org.BusinessUnitId,
                    OrganizationName = org.NameTh,
                      Description = org.DescriptionTh,
                    ParentOrganizationId = org.ParentId,
                    FlagDelete = "N",
                    FlagActive = true,
                    CreateBy = "system",
                    CreateDate = DateTime.Now,
                    UpdateBy = "system",
                    UpdateDate = DateTime.Now
                };
                if (existing == null)
                {
                    _context.MOrganizations.Add(mOrganization);
                }
                else
                {
                    _context.Entry(existing).CurrentValues.SetValues(mOrganization);
                }

            }

            await _context.SaveChangesAsync();
        }
    }
}