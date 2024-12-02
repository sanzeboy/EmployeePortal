using EmployeePortal.Application.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EmployeePortal.Application.Infastructures
{
    public interface IExcelService
    {
        FileDetail Export<T>(IList<T> exportData, string fileName, bool appendDateTimeInFileName = false, string sheetName = "Sheet1");
        ImportResponse<T> Import<T>(IFormFile file) where T : class;
    }
}