
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Convertor;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    public async Task<IActionResult> GetEmployeeList([FromQuery] PaginationQueryParamater paramater)
    {
        PaginatedList<EmployeeDetailDTO> employees = await _employeeService.GetEmployees(paramater);

        return Ok
            (
                SuccessResponse<PaginatedList<EmployeeDetailDTO>>.Create(
                    data: employees,
                    message: "Employees retrived successfully"
                )
            );
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
            throw new DataNotFoundException($"Employee with ID {id} not found");
        }

        return Ok
             (
                 SuccessResponse<EmployeeDetailDTO>.Create(
                     data: employee,
                     message: "Employee retrived successfully"
                 )
             );
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
            var validationDetails = ValidationConvertor.ConvertModelStateToValidationDetails(ModelState);
            throw new DataValidationException(validationDetails, "validation failed");
        }

        EmployeeDetailDTO? createdEmployeeDetails = await _employeeService.AddEmployee(newEmployee) ?? throw new DataConflictException("Employee with this email already exists");

        return CreatedAtAction(
           nameof(GetEmployeeById),
           new { id = createdEmployeeDetails.Id },
           SuccessResponse<EmployeeDetailDTO>.Create(
               data: createdEmployeeDetails,
               message: "Employee created successfully",
               statusCode: 201
           )
       );
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
            throw new DataValidationException("Id", "ID in route does not match ID in request body");
        }

        if (!ModelState.IsValid)
        {
            var validationDetails = ValidationConvertor.ConvertModelStateToValidationDetails(ModelState);
            throw new DataValidationException(validationDetails, "Validation failed");
        }

        EmployeeDetailDTO? updatedEmployeeDetails = await _employeeService.UpdateEmployee(updatedEmployee);

        if(updatedEmployeeDetails == null)
        {
            throw new DataNotFoundException($"Employee with ID {id} not found or could not be updated");
        }

        return Ok
            (SuccessResponse<EmployeeDetailDTO>.Create(
            data: updatedEmployeeDetails,
            message: "Employee updated successfully"));
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
            throw new DataNotFoundException($"Employee with ID {id} not found");
        }
        await _employeeService.DeleteEmployee(id);

        return Ok(SuccessResponse<bool>.Create(
                    data: true,
                   message: "Employee deleted successfully"
               ));
    }
}
