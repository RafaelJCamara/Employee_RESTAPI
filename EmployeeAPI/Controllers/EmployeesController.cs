using EmployeeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _db;

        public EmployeesController(IEmployeeRepository db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _db.GetEmployees());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database!");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployeeByID(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("ID must be greater than zero.");

                var employee = await _db.GetEmployee(id);

                if (employee == null) return NotFound("Employee not found!");

                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null) return BadRequest("Employee can not be null!");
            try
            {
                var searchByEmail = await _db.GetEmployeeByEmail(employee.Email);
                if (searchByEmail != null)
                {
                    ModelState.AddModelError("Email", "Employee email already registered!");
                    return BadRequest(ModelState);
                }

                var createdEmployee = await _db.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployeeByID), new { id = createdEmployee.EmployeeId }, employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database!");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (id <= 0) return BadRequest("ID must be greater than zero!");
            try
            {
                var employee = await _db.GetEmployee(id);
                if (employee == null) return NotFound("Employee not found!");
                await _db.DeleteEmployee(id);
                return Ok($"Employee with ID {id} deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database!");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (employee == null) return BadRequest("Please provide valid employee data!");
                if (id != employee.EmployeeId) return BadRequest("IDs do not match!");
                var updatedEmployee = await _db.UpdateEmployee(employee);
                if (updatedEmployee == null) return NotFound("Employee not found!");
                return Ok(updatedEmployee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database!");
            }
        }

    }
}
