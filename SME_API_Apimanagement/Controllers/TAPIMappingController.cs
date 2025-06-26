using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Service;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class TAPIMappingController : ControllerBase
    {
        private readonly ITAPIMappingRepository _repository;
        private readonly ApiMappingService _apiMappingService;
        public TAPIMappingController(ITAPIMappingRepository repository,ApiMappingService apiMappingService)
        {
            _repository = repository;
            _apiMappingService = apiMappingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mappings = await _repository.GetAllAsync();
            return Ok(mappings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mapping = await _repository.GetByIdAsync(id);
            return mapping != null ? Ok(mapping) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TApiPermisionMapping mapping)
        {
            mapping.FlagDelete = "N";
            mapping.CreateDate = DateTime.UtcNow;
            await _repository.AddAsync(mapping);
            return CreatedAtAction(nameof(GetById), new { id = mapping.Id }, mapping);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TApiPermisionMapping mapping)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.OrganizationCode = mapping.OrganizationCode;
            existing.SystemCode = mapping.SystemCode;
            existing.ApiKey = mapping.ApiKey;
        
            existing.StartDate = mapping.StartDate;
            existing.EndDate = mapping.EndDate;
            existing.FlagActive = mapping.FlagActive;
            existing.UpdateBy = mapping.UpdateBy;
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
        [Route("GetTApiMappingBySearch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<TApiPermisionMappingModels>>> GetTApiMappingBySearch(TApiPermisionMappingModels xModels)
        {
            try
            {
                var xdata = await _repository.GetTApiMappingBySearch(xModels); // ใช้ await
                if (xdata == null)
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

        [HttpPost("GetAPIList")]
        public async Task<IActionResult> GetAllByBusinessIdAsync(searchApiPermisionRespone models)
        {
            var result = new ApiPermisionApiRespone();
       
            result = await _apiMappingService.GetAllByBusinessIdAsync(models);
            return result != null ? Ok(result) : NotFound();
        }
    }

}
