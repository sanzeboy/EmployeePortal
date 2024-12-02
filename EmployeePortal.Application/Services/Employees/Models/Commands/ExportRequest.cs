namespace EmployeePortal.Application.Services.Employees.Models.Commands
{
    public class ExportRequest
    {
        public List<int> Ids { get; set; }
        public string Format { get; set; }
    }
}
