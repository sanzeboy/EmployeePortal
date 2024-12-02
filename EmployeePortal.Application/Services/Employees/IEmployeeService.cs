using EmployeePortal.Application.Services.Employees.Models.Commands;
using EmployeePortal.Application.Services.Employees.Models.Dtos;
using EmployeePortal.Application.Services.Employees.Models.Filters;
using EmployeePortal.Application;
using EmployeePortal.Application.Models;
using Microsoft.AspNetCore.Http;

namespace EmployeePortal.Application.Services.Employees
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<string> GetEmployeeDataFromExternalApi();
        Task<PagedResult<EmployeeDto>> GetEmployees(EmployeeFilter filter);
        Task<ImportResponse<ImportExportEmployee>> PreviewImport(IFormFile file);
        Task<ImportResponse<ImportExportEmployee>> SaveImport(IFormFile file);
        Task<FileDetail> Export(ExportRequest request);
        Task<bool> InsertEmployeeProfileImage(InsertEmployeeProfileImage profileImage);
        Task<int> InsertUpdateEmployee(InsertUpdateEmployee request);
        FileDetail SampleImportEmployeeFile();
    }
}