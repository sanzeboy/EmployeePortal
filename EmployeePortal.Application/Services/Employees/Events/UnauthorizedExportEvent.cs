using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Application.Services.Employees.Events
{
    public class UnauthorizedExportEvent
    {
        public DateTime AttemptedOn { get; set; }
        public string Message { get; set; }
    }
}
