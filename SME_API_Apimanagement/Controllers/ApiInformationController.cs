using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class ApiInformationController : ControllerBase
    {
        private readonly IApiInformationRepository _repository;

        public ApiInformationController(IApiInformationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MApiInformation>>> GetAll(string serviceCode,string servicename)
        {
            MapiInformationModels param = new MapiInformationModels
            {
                ServiceNameCode = serviceCode,
                ServiceNameTh = servicename
            };
            var items = await _repository.GetAllAsync(param);
            return Ok(items);
        }

        [HttpGet("{serviceCode}")]
        public async Task<ActionResult<MApiInformation>> GetById(string serviceCode)
        {
            var item = await _repository.GetByIdAsync(serviceCode);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MApiInformation model)
        {
            await _repository.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] MApiInformation model)
        {
            if (id != model.Id)
                return BadRequest();
            await _repository.UpdateAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}