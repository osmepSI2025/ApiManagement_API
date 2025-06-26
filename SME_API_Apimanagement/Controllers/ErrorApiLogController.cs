using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Repository;
using System.Text.Json;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorApiLogController : ControllerBase
    {
        private readonly IErrorApiLogRepository _repository;

        public ErrorApiLogController(IErrorApiLogRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var logs = await _repository.GetAllAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var log = await _repository.GetByIdAsync(id);
                return log != null ? Ok(log) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TErrorApiLog errorLog)
        {
            try
            {
                if (!string.IsNullOrEmpty(errorLog.HttpCode)) 
                {
                    var jsonData = JsonSerializer.Serialize(errorLog);
                    //    errorLog.Createdate = DateTime.UtcNow;
                    await _repository.AddAsync(errorLog);
                    return CreatedAtAction(nameof(GetById), new { id = errorLog.Id }, errorLog);
                }
                return CreatedAtAction(nameof(GetById), new { id = errorLog.Id }, errorLog);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TErrorApiLog errorLog)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null) return NotFound();

                existing.SystemCode = errorLog.SystemCode;
                existing.Message = errorLog.Message;
                existing.StackTrace = errorLog.StackTrace;
                existing.Source = errorLog.Source;
                existing.TargetSite = errorLog.TargetSite;
                existing.ErrorDate = errorLog.ErrorDate;
                existing.UserName = errorLog.UserName;
                existing.Path = errorLog.Path;
                existing.HttpMethod = errorLog.HttpMethod;
                existing.RequestData = errorLog.RequestData;
                existing.InnerException = errorLog.InnerException;
                existing.CreatedBy = errorLog.CreatedBy;

                await _repository.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
