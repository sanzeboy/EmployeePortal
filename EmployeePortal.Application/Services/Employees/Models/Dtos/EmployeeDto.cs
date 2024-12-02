using EmployeePortal.Domain.Entities.Employees;

namespace EmployeePortal.Application.Services.Employees.Models.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public GenderEnum Gender { get; set; }
        public decimal Salary { get; set; }
        public string ProfileImage { get; set; }
    }
}
