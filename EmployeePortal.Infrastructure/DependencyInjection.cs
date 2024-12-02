using EmployeePortal.Application.Infastructures;
using EmployeePortal.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmployeePortal.Application.Infrastructures;
using EmployeePortal.Infrastructure.Services;

namespace EmployeePortal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DatabaseName");

            // Add DbContext to DI
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString));

            services.AddTransient<ApplicationDbContext>();
            services.AddTransient<IApplicationDbContext,ApplicationDbContext>();
            services.AddSingleton<IDateTime, DateTimeService>();
            services.AddSingleton<IExcelService, ExcelService>();
            services.AddSingleton<IPdfService, PdfService>();
            services.AddSingleton<ICsvService, CsvService>();

            return services;
        }

    }
}
