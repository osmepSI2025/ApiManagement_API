using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Services;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class TSystemAPIController : ControllerBase
    {
        private readonly ITSystemAPIRepository _repository;
        private readonly IMSystemInfoService _mSystemInfoService;

        public TSystemAPIController(ITSystemAPIRepository repository
            , IMSystemInfoService mSystemInfoService
            )
        {
            _repository = repository;
            _mSystemInfoService = mSystemInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apis = await _repository.GetAllAsync();
            return Ok(apis);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var api = await _repository.GetByIdAsync(id);
            return api != null ? Ok(api) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TSystemApiMapping api)
        {
            api.FlagDelete = "N";
            api.CreateDate = DateTime.UtcNow;
            await _repository.AddAsync(api);
            return CreatedAtAction(nameof(GetById), new { id = api.Id }, api);
        }
        // 📌 POST: เพิ่มข้อมูล
        [HttpPost]
        [Route("UpsertSystemApi")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> UpsertSystemApi(UpSertSystemApiModels xModels)
        {
            try
            {
                // CheckSysitemInfoUpsert
                var msystemInfo = new MSystemInfoModels
                {
                    SystemCode = xModels.TSystemAPI.OwnerSystemCode,
                    ApiKey = xModels.TSystemAPI.ApiKey,
                    Note = xModels.TSystemAPI.ApiNote,
                    CreateBy = xModels.TSystemAPI.CreateBy,
                    ApiPassword = xModels.TSystemAPI.ApiPassword,
                    ApiUrlProdInbound = xModels.TSystemAPI.ApiUrlProdInbound,
                    ApiUrlUatInbound = xModels.TSystemAPI.ApiUrlUatInbound
                    ,
                    ApiUser = xModels.TSystemAPI.ApiUser
                    ,
                    FlagActive = xModels.TSystemAPI.FlagActive
                    ,
                    FlagDelete = xModels.TSystemAPI.FlagDelete
                    ,
                    CreateDate = xModels.TSystemAPI.CreateDate
                    ,
                    UpdateBy = xModels.TSystemAPI.UpdateBy
                    ,
                    UpdateDate = xModels.TSystemAPI.UpdateDate
                    ,
                    Id = xModels.TSystemAPI.Id
                };
         //       var systemInfo = await _mSystemInfoService.CheckSysitemInfoUpsert(msystemInfo);

                int xdata = await _repository.UpsertSystemApi(xModels); // ใช้ await

                return Ok(xdata); // คืนค่า 200 พร้อมข้อมูล
            }
            catch (Exception ex)
            {
                return BadRequest(); // ถ้ามีข้อผิดพลาดให้คืน 400
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TSystemApiMapping api)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.OwnerSystemCode = api.OwnerSystemCode;
            existing.ApiServiceName = api.ApiServiceName;
            existing.ApiMethod = api.ApiMethod;
            //existing.ApiUrlProd = api.ApiUrlProd;
            //existing.ApiUrlUat = api.ApiUrlUat;
            existing.ApiKey = api.ApiKey;
            existing.ApiRequestExample = api.ApiRequestExample;
            existing.ApiResponseExample = api.ApiResponseExample;
            existing.ApiNote = api.ApiNote;
            existing.FlagActive = api.FlagActive;
            existing.UpdateBy = api.UpdateBy;
            existing.UpdateDate = DateTime.UtcNow;
            existing.ApiUrlProdInbound = api.ApiUrlProdInbound;
            existing.ApiUrlUatInbound = api.ApiUrlUatInbound;
            existing.ApiUser = api.ApiUser;
            existing.ApiPassword = api.ApiPassword;
            existing.ApiServiceType = api.ApiServiceType;
            existing.ApiUrlProdOutbound = api.ApiUrlProdOutbound;
            existing.ApiUrlUatOutbound = api.ApiUrlUatOutbound;
            

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result ? Ok() : BadRequest();
        }

        [HttpPost]
        [Route("GetTSystemMappingBySearch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetTSystemMappingBySearch(TSystemApiMappingModels xModels)
        {
            try
            {
                var xdata = await _repository.GetTSystemMappingBySearch(xModels); // ใช้ await
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

    }

}
