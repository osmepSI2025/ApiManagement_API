using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Services;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class TEmployeeLineOaController : ControllerBase
    {
        private readonly TEmployeeLineOaService _service;

        public TEmployeeLineOaController(TEmployeeLineOaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string EmpId)
        {
            try
            {
                var result = await _service.GetByIdAsync(EmpId);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TEmployeeLineOa entity)
        {
            // to upsert empline
            try
            {

                var emplist = _service.GetByIdAsync(entity.EmployeeId);
                if (emplist.Result != null)
                {
                    // Update existing employee line
                    var existingEntity = await emplist;
                    if (existingEntity != null)
                    {
                        existingEntity.EmployeeId = entity.EmployeeId;
                        existingEntity.LineOaDateJoined = entity.LineOaDateJoined;

                        existingEntity.LineOaPictureUrl = entity.LineOaPictureUrl;
                        existingEntity.LineOaRefreshToken = entity.LineOaRefreshToken;
                        existingEntity.LineOaAccessToken = entity.LineOaAccessToken;


                        // Update the existing entity

                        var success = await _service.UpdateAsync(existingEntity);
                      
                    }
                }
                else
                {
                    var success = await _service.AddAsync(entity);
                    
                }

                return Ok(entity);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TEmployeeLineOa entity)
        {
            try
            {
                if (id != entity.Id) return BadRequest();
                var success = await _service.UpdateAsync(entity);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}