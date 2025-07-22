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

    /// <summary>
    /// Retrieves a list of all employees.
    /// </summary>
    /// <returns>An ActionResult containing a list of EmployeeDetailDTO objects.
    /// </returns>
    /// <remarks>
    /// **Route:** GET /api/Employee
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetEmployeeList()
    {
        List<EmployeeDetailDTO> employees = await _employeeService.GetEmployees();
        return Ok(employees);
    }

    /// <summary>
    /// Retrieves a single employee by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>An ActionResult containing an EmployeeDetailDTO object or a NotFound result.</returns>
    /// <remarks>
    /// **Route:** GET /api/Employee/{id}
    /// </remarks>
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

    /// <summary>
    /// Creates a new employee record.
    /// </summary>
    /// <param name="newEmployee">The data for the new employee, sent in the request body.</param>
    /// <returns>An ActionResult indicating the result of the creation operation.</returns>
    /// <remarks>
    /// **Route:** POST /api/Employee
    /// </remarks>
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

    /// <summary>
    /// Updates an existing employee record by ID.
    /// </summary>
    /// <param name="id">The ID of the employee to update, from the route.</param>
    /// <param name="updatedEmployee">The updated employee data, sent in the request body.</param>
    /// <returns>An ActionResult indicating the result of the update operation.</returns>
    /// <remarks>
    /// **Route:** PUT /api/Employee/{id}
    /// </remarks>
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

        EmployeeDetailDTO? updatedEmployeeDetails = await _employeeService.UpdateEmployee(updatedEmployee);
        if(updatedEmployeeDetails == null)
        {
            return NotFound($"Employee with ID {id} not found or could not be updated.");
        }

        return Ok(updatedEmployeeDetails);
    }

    /// <summary>
    /// Deletes an employee record by ID.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    /// <returns>An ActionResult indicating the result of the deletion operation.</returns>
    /// <remarks>
    /// **Route:** DELETE /api/Employee/{id}
    /// </remarks>
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
