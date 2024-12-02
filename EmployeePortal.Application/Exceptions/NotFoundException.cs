using System.Net;

namespace EmployeePortal.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message, string description = "") : base(message, description, (int)HttpStatusCode.NotFound)
        {
        }
    }
}
