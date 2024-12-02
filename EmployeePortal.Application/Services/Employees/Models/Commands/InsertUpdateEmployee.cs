using EmployeePortal.Domain.Entities.Employees;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Application.Services.Employees.Models.Commands
{
    public class InsertUpdateEmployee
    {
        public int Id { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}
