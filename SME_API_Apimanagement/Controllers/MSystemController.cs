using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class MSystemController : ControllerBase
    {
        private readonly IMSystemRepository _repository;

        public MSystemController(IMSystemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var systems = await _repository.GetAllAsync();
            return Ok(systems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var system = await _repository.GetByIdAsync(id);
            return system != null ? Ok(system) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MSystem system)
        {
            system.FlagDelete = "N";
            system.CreateDate = DateTime.UtcNow;
            await _repository.AddAsync(system);
            return CreatedAtAction(nameof(GetById), new { id = system.Id }, system);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MSystem system)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.SystemCode = system.SystemCode;
            existing.SystemName = system.SystemName;
            existing.FlagActive = system.FlagActive;
            existing.UpdateBy = system.UpdateBy;
            existing.UpdateDate = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
           var xresult = await _repository.DeleteAsync(id);

            return xresult;
        }

        [HttpPost]
        [Route("GetSystemBySearch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<MSystemModels>>> GetSystemBySearch(MSystemModels xModels)
        {
            try
            {
                var xdata = await _repository.GetSystemBySearch(xModels); // ใช้ await
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
        [Route("UpsertSystem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> UpsertSystem(MSystemModels xModels)
        {
            try
            {
                int xdata = await _repository.UpsertSystem(xModels); // ใช้ await

                return Ok(xdata); // คืนค่า 200 พร้อมข้อมูล
            }
            catch (Exception ex)
            {
                return BadRequest(); // ถ้ามีข้อผิดพลาดให้คืน 400
            }
        }
    }

}
