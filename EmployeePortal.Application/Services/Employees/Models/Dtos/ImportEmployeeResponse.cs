using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Application.Services.Employees.Models.Dtos
{
    public class ImportEmployeeResponse
    {
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; }
        public List<ImportExportEmployee> Data { get; set; }
        public ImportEmployeeResponse()
        {
            ErrorMessages = new List<string>();
            Data = new List<ImportExportEmployee>();
        }
    }
}
