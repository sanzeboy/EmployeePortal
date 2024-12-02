using Microsoft.EntityFrameworkCore;
using EmployeePortal.Application.Infrastructures;
using EmployeePortal.Domain.Entities;
using EmployeePortal.Domain.Entities.AppUsers;
using EmployeePortal.Domain.Entities.Employees;
using System.Linq.Expressions;

namespace EmployeePortal.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;


        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.ApplyAllConfiguration();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppUserConfiguration).Assembly);

            // Configure global query filter for all entities with IsDeleted property
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(CommonProperties).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(CommonProperties.IsDeleted));
                    var filter = Expression.Lambda(
                        Expression.Not(property),
                        parameter
                    );

                    entityType.SetQueryFilter(filter);
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }

        public Task<int> SaveChangesAsync()
        {
            foreach (var entry in ChangeTracker.Entries<CommonProperties>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity.CreatedByAppUserId == 0)
                            entry.Entity.CreatedByAppUserId = _currentUserService.IsAuthenticated ? _currentUserService.AppUserId : 0;
                        entry.Entity.CreatedOn = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        if (entry.Entity.ModifiedBy == 0)
                            entry.Entity.ModifiedBy = _currentUserService.IsAuthenticated ? _currentUserService.AppUserId : null;
                        entry.Entity.ModifiedOn = _dateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync();
        }

        public override Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

      
    }
}
