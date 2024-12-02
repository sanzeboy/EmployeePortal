using EmployeePortal.Application.Services.Employees.Models.Dtos;
using EmployeePortal.Application.Services.Employees.Models.Filters;
using EmployeePortal.Application;

namespace EmployeePortal.UI.Models
{
    public class EmployeeViewModel
    {
        public EmployeeFilter Filter { get; set; }
        public PagedResult<EmployeeDto> Data { get; set; }

    }
}
