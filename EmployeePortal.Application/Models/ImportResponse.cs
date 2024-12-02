using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePortal.Application.Models
{
    public class ImportResponse<T> where T : class
    {
        public bool Success { get; set; }
        public List<T> Data { get; set; }
        public List<string> ErrorMessages { get; set; }
        public List<int> SkippedRows { get; set; }
        public ImportResponse()
        {
            ErrorMessages = new List<string>();
            SkippedRows = new List<int>();
            Data = new List<T>();
        }
    }
}
