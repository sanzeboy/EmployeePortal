namespace EmployeePortal.Application.Services.AppUsers.Models.Dtos
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? AppUserId { get; set; }
    }
}
