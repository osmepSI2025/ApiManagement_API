using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Services;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class MOrganizationController : ControllerBase
    {
        private readonly IMOrganizationRepository _repository;
        private readonly MOrganizationService _service;

        public MOrganizationController(IMOrganizationRepository repository
            , MOrganizationService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizations = await _repository.GetAllAsync();
            return Ok(organizations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var organization = await _repository.GetByIdAsync(id);
            return organization != null ? Ok(organization) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MOrganization organization)
        {
            organization.FlagDelete = "N";
            organization.CreateDate = DateTime.UtcNow;
            await _repository.AddAsync(organization);
            return CreatedAtAction(nameof(GetById), new { id = organization.OrganizationId }, organization);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MOrganization organization)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.OrganizationCode = organization.OrganizationCode;
            existing.OrganizationName = organization.OrganizationName;
            existing.Description = organization.Description;
            existing.ParentOrganizationId = organization.ParentOrganizationId;
            existing.TaxId = organization.TaxId;
            existing.IndustryType = organization.IndustryType;
            existing.Email = organization.Email;
            existing.PhoneNumber = organization.PhoneNumber;
            existing.Address = organization.Address;
            existing.City = organization.City;
            existing.StateOrProvince = organization.StateOrProvince;
            existing.Country = organization.Country;
            existing.PostalCode = organization.PostalCode;
            existing.WebsiteUrl = organization.WebsiteUrl;
            existing.LogoUrl = organization.LogoUrl;
            existing.Status = organization.Status;
            existing.FlagActive = organization.FlagActive;
            existing.UpdateBy = organization.UpdateBy;
            existing.UpdateDate = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost]
        [Route("GetOrgBySeach")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<MOrganizationModels>>> GetOrgBySeach(MOrganizationModels xModels)
        {
            try
            {
                var xdata = await _repository.GetOrgBySeach(xModels); // ใช้ await
                if (xdata == null || !xdata.Any())
                {
                    return NotFound(); // หากไม่พบข้อมูล, คืนค่า 404
                }
                return Ok(xdata); // คืนค่า 200 พร้อมข้อมูล
            }
            catch (Exception ex)
            {
                // อาจจะใส่ log หรือรายละเอียดเพิ่มเติมของข้อผิดพลาดใน ex
                return BadRequest(new { message = ex.Message }); // คืน 400 พร้อมข้อความข้อผิดพลาด
            }
        }

        [HttpPost]
        [Route("UpsertOrg")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> UpsertOrg(MOrganizationModels xModels)
        {
            try
            {
                int xdata = await _repository.UpsertOrg(xModels); // ใช้ await

                return Ok(xdata); // คืนค่า 200 พร้อมข้อมูล
            }
            catch (Exception ex)
            {
                return BadRequest(); // ถ้ามีข้อผิดพลาดให้คืน 400
            }
        }

        [HttpGet]
        [Route("Organization-Batch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> OrgBatch()
        {
            try
            {
                await _service.UpsertAsync(); // Call UpsertAsync from MOrganizationService
                return Ok(1); // Return 200 with a success value
            }
            catch (Exception ex)
            {
                return BadRequest(); // Return 400 in case of an error
            }
        }
    }

}
