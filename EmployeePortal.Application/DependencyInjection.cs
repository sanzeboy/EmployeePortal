using Microsoft.Extensions.DependencyInjection;
using EmployeePortal.Application.Services.AppUsers;
using Microsoft.Extensions.Configuration;
using EmployeePortal.Application.Models;
using EmployeePortal.Application.Events;
using EmployeePortal.Application.Services.Employees.Events;

namespace EmployeePortal.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            var allServices = typeof(IAppUserService).Assembly.DefinedTypes;
            foreach (var intfc in allServices.Where(t => t.IsInterface))
            {
                var impl = allServices.FirstOrDefault(c => c.IsClass && intfc.Name.Substring(1) == c.Name);
                if (impl != null)
                    services.AddScoped(intfc, impl);
            }


            // Add Events
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IEventSubscriber<UnauthorizedExportEvent>, UnauthorizedExportEventHandler>();
            return services;


        }

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(appSettingsSection);

          

        }


    }
}
