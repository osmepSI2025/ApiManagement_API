using Microsoft.AspNetCore.Mvc;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Service;


namespace SME_API_Apimanagement.Controllers
{
    [Route("api/SYS-API/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManagementService _service;
        private readonly HrEmployeeService _hrEmployeeService;
        public UserManagementController(UserManagementService service
            , HrEmployeeService hrEmployeeService)
        {
            _service = service;
            _hrEmployeeService = hrEmployeeService;
        }


        [HttpGet()]
        public async Task<ActionResult<List<TEmployeeRole>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEmployeeRole>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] EmployeeRoleModels entity)
        {
            TEmployeeRole newEntity = new TEmployeeRole
            {
                EmployeeCode = entity.EmployeeId,
                RoleCode = entity.EmployeeRole,
                BusinessUnitId = entity.BusinessUnitId,
                PositionId = entity.PositionId,
                CreateBy = "System", // Replace with actual user context
                CreateDate = DateTime.UtcNow
            };
            await _service.AddAsync(newEntity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TEmployeeRole entity)
        {
            if (id != entity.Id) return BadRequest();
            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var xereult = await _service.DeleteAsync(id);
            return xereult;
        }

        [HttpGet("GetEmployeeProfileList")]
        public async Task<ActionResult> GetEmployeeProfileList()
        {

            var lEmp = await _hrEmployeeService.GetEmployeeList();


            // var DepartmentProfile = await _service.GetAllAsync();
            return Ok(lEmp);
        }

        [HttpPost("GetAllEmployeeList")]
        public async Task<ActionResult> GetAllEmployeeList(searchEmployeeRoleModels models)
        {

            var vm = await _hrEmployeeService.GetAllEmployeeList(models);



            // var DepartmentProfile = await _service.GetAllAsync();
            return Ok(vm);
        }
        [HttpPost("GetEmployeeDetail")]
        public async Task<ActionResult> GetEmployeeDetail(EmployeeResult models)
        {

            var vm = await _hrEmployeeService.GetEmployeeDetailBySearch(models);



            // var DepartmentProfile = await _service.GetAllAsync();
            return Ok(vm);
        }
    }
}