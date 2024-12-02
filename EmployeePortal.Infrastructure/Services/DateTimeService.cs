using EmployeePortal.Application.Infrastructures;

namespace EmployeePortal.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now
        {
            get
            {
                // Server time is at Pacific standard time, so convert it to nepali standard time
                var nepaliTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time");
                return TimeZoneInfo.ConvertTime(DateTime.Now, nepaliTimeZone);
            }
        }
      
    }
}
