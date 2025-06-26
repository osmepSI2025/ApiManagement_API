using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using System.Text;

[Route("api/SYS-API/[controller]")]
[ApiController]
public class MRegisterController : ControllerBase
{
    private readonly IMRegisterRepository _repository;
    private readonly ITAPIMappingRepository _repTApi;


    public MRegisterController(IMRegisterRepository repository, ITAPIMappingRepository repTApi)
    {
        _repository = repository;
        _repTApi = repTApi;
    }

    // 📌 GET: ดึงข้อมูลทั้งหมด
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MRegister>>> GetRegisters()
    {
        var registers = await _repository.GetRegistersAsync();
        return Ok(registers);
    }

    // 📌 GET: ดึงข้อมูลตาม apikey และ xUrl
    //[HttpGet("{apikey}/{xUrl}")]
    //public async Task<ActionResult<MRegister>> GetRegister(string apikey,string xUrl)
    //{
    //    var register = await _repository.GetRegisterByIdAsync(apikey);
    //    return register == null ? NotFound() : Ok(register);
    //}
    [HttpGet("{apikey}/{xUrl}")]
    public async Task<ActionResult<MRegister>> GetRegister(string apikey, string xUrl)
    {
        string decodedPath;
        try
        {
            decodedPath = Encoding.UTF8.GetString(Convert.FromBase64String(xUrl));
        }
        catch
        {
            return BadRequest("xUrl is not a valid Base64 string.");
        }

        var register = await _repository.GetRegisterByIdAsync(apikey);
        if (register == null)
            return NotFound();

        var xparam = new TApiPermisionMappingModels { ApiKey = apikey };
        var tmapping = await _repTApi.GetCheckTApiMappingBySearch(xparam);

        if (tmapping != null && tmapping.Count > 0)
        {
            var uniqueMappings = tmapping
                .Select(x => new { x.ApiServiceCode, x.SystemCode,x.FlagActive })
                .Distinct()
                .ToList();

            foreach (var item in uniqueMappings)
            {
                bool hasApi = decodedPath.Contains(item.ApiServiceCode, StringComparison.OrdinalIgnoreCase);
                bool hasSys = decodedPath.Contains(item.SystemCode, StringComparison.OrdinalIgnoreCase);

                if (hasApi && hasSys && item.FlagActive == true)
                {
                    return Ok(register);
                }
            }
        }

        return Forbid(); // ไม่มีสิทธิ์
    }



    // 📌 POST: เพิ่มข้อมูล
    [HttpPost]
    [Route("UpsertRegister")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<int>> UpsertRegister(UpSertRegisterApiModels xModels)
    {
        try
        {
            string xdata = await _repository.UpdateOrInsertRegister(xModels); // ใช้ await
            if (xdata != "")
            {
                int TApi = await _repTApi.UpdateOrInsertTApiMapping(xModels, xdata);
            }
            return Ok(200); // คืนค่า 200 พร้อมข้อมูล
        }
        catch (Exception ex)
        {
            return BadRequest(); // ถ้ามีข้อผิดพลาดให้คืน 400
        }
    }
    // 📌 POST: เพิ่มข้อมูล
    [HttpPost]
    [Route("GetRegisterBySearch")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> GetRegisterBySearch(MRegisterModels xModels)
    {
        try
        {
            var xdata = await _repository.GetRegisterBySearch(xModels); // ใช้ await
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

    // 📌 PUT: อัปเดตข้อมูล
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRegister(int id, MRegister register)
    {
        if (id != register.Id) return BadRequest();

        await _repository.UpdateRegisterAsync(register);
        return NoContent();
    }

    // 📌 DELETE: ลบข้อมูล
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRegister(int id)
    {
        await _repository.DeleteRegisterAsync(id);
        return NoContent();
    }
}

