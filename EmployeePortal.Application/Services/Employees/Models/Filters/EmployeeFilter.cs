using EmployeePortal.Application;
using EmployeePortal.Domain.Entities.Employees;

namespace EmployeePortal.Application.Services.Employees.Models.Filters
{
    public class EmployeeFilter : PaginationFilter
    {
        public DateTime? DobFrom { get; set; }
        public DateTime? DobTo { get; set; }
        public GenderEnum? Gender { get; set; }
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
    }
}
