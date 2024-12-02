using EmployeePortal.Application.Events;
using EmployeePortal.Application.Services.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Application.Services.Employees.Events
{
   
    public class UnauthorizedExportEventHandler : IEventSubscriber<UnauthorizedExportEvent>
    {
        private readonly IEmailService _emailService;

        public UnauthorizedExportEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task HandleAsync(UnauthorizedExportEvent @event)
        {
            var adminEmail = "admin@gmail.com";
            var emailSubject = "Unauthorized Access Attempt Detected";
            var emailBody = $@"
            <p>Dear Admin,</p>
            <p>An unauthorized user attempted to perform a restricted operation.</p>
            <p><strong>Details:</strong></p>
            <ul>
                <li>Message: {@event.Message}</li>
                <li>Attempted On: {@event.AttemptedOn}</li>
            </ul>
            <p>Please take necessary actions.</p>
            <p>Regards,</p>
            <p>Your System</p>";

            await _emailService.SendEmailAsync(adminEmail, emailSubject, emailBody);
        }
    }

}
