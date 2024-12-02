using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace EmployeePortal.Application.Infrastructures
{
    public interface ICurrentUserService
    {
        public bool IsAuthenticated { get; }
        public HttpContext HttpContext { get; }
        public int AppUserId { get; }
    }
}
