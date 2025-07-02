using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Services;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class TEmployeeMapSystemController : ControllerBase
    {
        private readonly TEmployeeMapSystemService _service;

        public TEmployeeMapSystemController(TEmployeeMapSystemService service)
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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TEmployeeMapSystem entity)
        {
            try
            {
                var success = await _service.AddAsync(entity);
                if (!success) return BadRequest();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TEmployeeMapSystem entity)
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
        public async Task<IActionResult> Delete(string id)
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

        [HttpGet("EmaployeeId={Emaployeeid}")]
        public async Task<IActionResult> GetByEmpId(string Emaployeeid)
        {
            try
            {
                var result = await _service.GetByEmpIdAsync(Emaployeeid);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("CreateEmpSystemList")]
        public async Task<IActionResult> CreateList([FromBody] List<TEmployeeMapSystem> entity)
        {
            try
            {
                // delete ก่อน เสมอ
                var del = await _service.DeleteAsync(entity[0].EmployeeId);

                var success = await _service.AddRangeAsync(entity);
                if (!success) return BadRequest();
                return Ok(1);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}