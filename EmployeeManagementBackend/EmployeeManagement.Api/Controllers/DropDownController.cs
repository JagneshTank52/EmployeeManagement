using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Services.DTO.DropDown;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DropDownController : ControllerBase
{
    private readonly IDropDownService _dropDownService;

    public DropDownController(IDropDownService dropDownService)
    {
        _dropDownService = dropDownService;
    }

    [HttpGet("get-drop-down-list/{type}")]
    public async Task<IActionResult> GetDropDownList([FromRoute] Enums.DropDownType type,[FromQuery] int? filterId)
    {
        List<DropDownListDTO> dropDownList = await _dropDownService.GetDropDownListsAsync(type,filterId);

        return Ok(
            SuccessResponse<List<DropDownListDTO>>.Create(
                data: dropDownList,
                message: Messages.Success.General.GetSuccess($"{type} for dropdown")
            )
        );
    }
}
