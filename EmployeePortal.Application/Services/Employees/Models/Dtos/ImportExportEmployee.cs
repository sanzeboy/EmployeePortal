using EmployeePortal.Domain.Entities.Employees;

namespace EmployeePortal.Application.Services.Employees.Models.Dtos
{
    public class ImportExportEmployee
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public decimal Salary { get; set; }
    }
}
