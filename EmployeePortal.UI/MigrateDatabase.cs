using Microsoft.EntityFrameworkCore;
using EmployeePortal.Application.HelperClasses;
using EmployeePortal.Infrastructure;

namespace EmployeePortal.UI
{
    public static class DatabaseMigration
    {
        public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
        {

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    await SeedUser(context);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return app;
        }


        public static async Task SeedUser(ApplicationDbContext context)
        {
            if (await context.AppUsers.AnyAsync())
                return;

            // Hash the passowrd
            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash("admin", out passwordHash, out passwordSalt);

            context.AppUsers.Add(new Domain.Entities.AppUsers.AppUser
            {
                Username = "admin",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            });
            await context.SaveChangesAsync();
        }
    }
}
