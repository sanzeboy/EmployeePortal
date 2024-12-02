using EmployeePortal.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePortal.Application
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int? PageSize { get; set; }
        
        public string SortBy { get; set; }
        public bool Desc { get; set; }
        public string Search { get; set; }
        public PaginationFilter()
        {
            // Default page number
            PageNumber = 1;
        }
    }
}
