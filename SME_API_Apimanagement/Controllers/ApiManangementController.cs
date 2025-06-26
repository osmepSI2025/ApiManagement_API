using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Models;

namespace SME_API_Apimanagement.Controllers
{
  
    [ApiController]
    [Route("api/SYS-API/[controller]")]
    public class ApiManangementController : ControllerBase
    {
        [HttpPost()]
        //[Authorize]
        public List<ApiModels> GetApiList(ApiModels model)
        {
            List<ApiModels> lApi = new List<ApiModels>();
            ApiModels m1 = new ApiModels();
            ApiModels m2 = new ApiModels();
            m1.Id = 1; m2.Id = 2;
            m1.Name = "Palmmy";
            m2.Name = "AI";
            m1.Description = "Hansome";
            m2.Description = "Someone";
            lApi.Add(m1);
            lApi.Add(m2);

            return lApi;
           
        }
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetApi(int id)
        {
            return Ok(new { UserId = id, Name = "John Doe" });
        }
    }
}
