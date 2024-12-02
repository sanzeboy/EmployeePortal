using Microsoft.AspNetCore.Http;

namespace EmployeePortal.Application.Services.Employees.Models.Commands
{
    public class InsertEmployeeProfileImage
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }

    }
}
