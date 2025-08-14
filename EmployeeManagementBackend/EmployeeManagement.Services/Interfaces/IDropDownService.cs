using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Services.DTO.DropDown;

namespace EmployeeManagement.Services.Interfaces;

public interface IDropDownService
{
    Task<List<DropDownListDTO>> GetDropDownListsAsync(Enums.DropDownType dropDownType,int? filterId);
}
