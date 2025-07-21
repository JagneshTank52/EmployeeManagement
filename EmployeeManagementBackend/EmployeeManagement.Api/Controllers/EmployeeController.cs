using System.Threading.Tasks;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeList()
    {
        List<EmployeeDetailDTO> employees = await _employeeService.GetEmployees();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        EmployeeDetailDTO? employee = await _employeeService.GetEmployeeById(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeDTO newEmployee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        EmployeeDetailDTO? createdEmployeeDetails = await _employeeService.AddEmployee(newEmployee);
        if (createdEmployeeDetails == null)
        {
            return Conflict("Employee with this email already exists.");
        }

         return CreatedAtAction(
            nameof(GetEmployeeById), 
            new { id = createdEmployeeDetails.Id }, 
            createdEmployeeDetails);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] AddEmployeeDTO updatedEmployee)
    {
        if (id != updatedEmployee.Id)
        {
            return BadRequest("ID in route does not match ID in request body.");
        }

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        EmployeeDetailDTO updatedEmployeeDetails = await _employeeService.UpdateEmployee(updatedEmployee);
        if(updatedEmployeeDetails == null)
        {
            return NotFound($"Employee with ID {id} not found or could not be updated.");
        }

        return Ok(updatedEmployeeDetails);
    }

    [HttpDelete("{id}")]        
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        EmployeeDetailDTO? employee =  await _employeeService.GetEmployeeById(id);
        if (employee == null)
        {
            return NotFound();
        }
        await _employeeService.DeleteEmployee(id);
        
        return NoContent(); 
    }
}
