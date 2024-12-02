using Microsoft.EntityFrameworkCore;
using EmployeePortal.Domain.Entities.AppUsers;
using EmployeePortal.Domain.Entities.Employees;

namespace EmployeePortal.Application.Infrastructures
{
    public interface IApplicationDbContext : IDisposable
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        Task<int> SaveChangesAsync();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry Entry(object entity);

    }
}
