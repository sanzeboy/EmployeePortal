namespace EmployeePortal.Domain.Entities.Employees
{
    public class Employee : CommonProperties
    {
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public GenderEnum Gender { get; set; }
        public decimal Salary { get; set; }
        public string? ProfileImage { get; set; }
    }
}
