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
    public class MSystemInfoController : ControllerBase
    {
        private readonly IMSystemInfoService _service;

        public MSystemInfoController(IMSystemInfoService service)
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{systemcode}")]
        public async Task<IActionResult> GetById(string systemcode)
        {
            try
            {
                var result = await _service.GetByIdAsync(systemcode);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MSystemInfo entity)
        {
            try
            {
                var result = await _service.AddAsync(entity);
                if (result > 0) return Ok(entity);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MSystemInfo entity)
        {
            try
            {
                if (id != entity.Id) return BadRequest();
                var result = await _service.UpdateAsync(entity);
                if (result > 0) return Ok(entity);
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
        // 📌 POST: เพิ่มข้อมูล
        [HttpPost]
        [Route("UpsertSystemInfo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> UpsertSystemInfo(MSystemInfoModels xModels)
        {
            try
            {
                // CheckSysitemInfoUpsert
                var msystemInfo = new MSystemInfoModels
                {
                    SystemCode = xModels.SystemCode,
                    ApiKey = xModels.ApiKey,
                    Note = xModels.Note,
                    CreateBy = xModels.CreateBy,
                    ApiPassword = xModels.ApiPassword,
                    ApiUrlProdInbound = xModels.ApiUrlProdInbound,
                    ApiUrlUatInbound = xModels.ApiUrlUatInbound
                    ,
                    ApiUser = xModels.ApiUser
                    ,
                    FlagActive = xModels.FlagActive
                    ,
                    FlagDelete = xModels.FlagDelete
                    ,
                    CreateDate = xModels.CreateDate
                    ,
                    UpdateBy = xModels.UpdateBy
                    ,
                    UpdateDate = xModels.UpdateDate
                    ,
                    Id = xModels.Id
                };
                var systemInfo = await _service.CheckSysitemInfoUpsert(msystemInfo);


                return Ok(systemInfo); // คืนค่า 200 พร้อมข้อมูล
            }
            catch (Exception ex)
            {
                return BadRequest(); // ถ้ามีข้อผิดพลาดให้คืน 400
            }
        }

    }
}