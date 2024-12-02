using EmployeePortal.Application.Services.AppUsers.Models.Dtos;

namespace EmployeePortal.Application.Services.AppUsers
{
    public interface IAppUserService
    {
        Task<LoginResponse> Login(LoginDto request);

    }
}
