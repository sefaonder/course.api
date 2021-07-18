using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using course.helper;
using course.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace course.controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("Add")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> Add([FromBody] EmployeeAddDto model)
        {
            var result = await _employeeService.Add(model);

            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EmployeeUpdateDto model)
        {
            var result = await _employeeService.Update(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([BindRequired] Guid id)
        {
            var result = await _employeeService.Delete(id);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(IList<EmployeeGetDto>), 200)]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeService.Get();

            return Ok(result);
        }
    }
}