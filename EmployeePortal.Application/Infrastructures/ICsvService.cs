using EmployeePortal.Application.Models;
using Microsoft.AspNetCore.Http;

namespace EmployeePortal.Application.Infrastructures
{
    public interface ICsvService
    {
        public FileDetail Export<T>(List<T> data, string fileName);
        public ImportResponse<T> Import<T>(IFormFile file, char delimiter = ',') where T : class, new();
    }
}
