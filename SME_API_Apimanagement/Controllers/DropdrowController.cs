using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;

namespace SME_API_Apimanagement.Controllers
{
    [ApiController]
    [Route("api/SYS-API/[controller]")]
    public class DropdownController : ControllerBase
    {
        private readonly ApiMangeDBContext _context;
        private readonly IDropdownRepository _Eg;
        public DropdownController(ApiMangeDBContext context, IDropdownRepository DropdownRepository)
        {
            _context = context;
            _Eg = DropdownRepository;
        }


        [HttpGet]
        [Route("LookUp/{LookUpType}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public vDropdownDTO GetDropdownLookUp(string LookUpType)
        {
            vDropdownDTO vList = new vDropdownDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    var lDropdown = _Eg.GetDropdownLookUp(LookUpType);
                    if (lDropdown.Count > 0)
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = lDropdown;
                        return vList;
                    }
                    else
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = null;
                        return vList;
                    }
                }
                else
                {
                    vList.responseCode = "RES-500";
                    vList.responseDesc = "Resquest processes Unsuccess";
                    return vList;
                }
            }
            catch (Exception ex)
            {
                vList.responseCode = "RES-404";
                vList.responseDesc = ex.Message;
                ;
                return vList;
            }
        }

        [HttpGet]
        [Route("GetDropdownSystem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public vDropdownDTO GetDropdownSystem()
        {
            vDropdownDTO vList = new vDropdownDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    var lDropdown = _Eg.GetDropdownSystem();
                    if (lDropdown.Count > 0)
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = lDropdown;
                        return vList;
                    }
                    else
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = null;
                        return vList;
                    }
                }
                else
                {
                    vList.responseCode = "RES-500";
                    vList.responseDesc = "Resquest processes Unsuccess";
                    return vList;
                }
            }
            catch (Exception ex)
            {
                vList.responseCode = "RES-404";
                vList.responseDesc = ex.Message;
                ;
                return vList;
            }
        }
        [HttpGet]
        [Route("GetDropdownOrganization")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public vDropdownDTO GetDropdownOrganization()
        {
            vDropdownDTO vList = new vDropdownDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    var lDropdown = _Eg.GetDropdownOrganization();
                    if (lDropdown.Count > 0)
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = lDropdown;
                        return vList;
                    }
                    else
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = null;
                        return vList;
                    }
                }
                else
                {
                    vList.responseCode = "RES-500";
                    vList.responseDesc = "Resquest processes Unsuccess";
                    return vList;
                }
            }
            catch (Exception ex)
            {
                vList.responseCode = "RES-404";
                vList.responseDesc = ex.Message;
                ;
                return vList;
            }
        }

        [HttpPost]
        [Route("GetDropdownOrganizationWithOutData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public vDropdownDTO GetDropdownOrganizationWithOutData(List<MOrganizationModels>  WoModels)
        {
            vDropdownDTO vList = new vDropdownDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    var lDropdown = _Eg.GetDropdownOrganizationWithOutData(WoModels);
                    if (lDropdown.Count > 0)
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = lDropdown;
                        return vList;
                    }
                    else
                    {
                        vList.responseCode = "RES-200";
                        vList.responseDesc = "Resquest processes successfully";
                        vList.DropdownList = null;
                        return vList;
                    }
                }
                else
                {
                    vList.responseCode = "RES-500";
                    vList.responseDesc = "Resquest processes Unsuccess";
                    return vList;
                }
            }
            catch (Exception ex)
            {
                vList.responseCode = "RES-404";
                vList.responseDesc = ex.Message;
                ;
                return vList;
            }
        }
    }
}
