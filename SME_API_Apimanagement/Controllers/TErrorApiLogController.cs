using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Services;
using System;
using System.Threading.Tasks;

namespace SME_API_Apimanagement.Controllers
{
    [ApiController]
    [Route("api/SYS-API/[controller]")]
    public class TErrorApiLogController : ControllerBase
    {
        private readonly ITErrorApiLogService _service;

        public TErrorApiLogController(ITErrorApiLogService service)
        {
            _service = service;
        }

        [HttpPost("GetErrorApi")]
        public async Task<IActionResult> GetAll(TErrorApiLogModels model)
        {
            try
            {
                var logs = await _service.GetAllAsync(model);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var log = await _service.GetByIdAsync(id);
                if (log == null) return NotFound();
                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TErrorApiLog log)
        {
            try
            {
                var result = await _service.AddAsync(log);
                if (result > 0) return Ok(log);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TErrorApiLog log)
        {
            try
            {
                if (id != log.Id) return BadRequest();
                var result = await _service.UpdateAsync(log);
                if (result > 0) return Ok(log);
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (result > 0) return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}