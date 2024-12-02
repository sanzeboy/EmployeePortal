using Microsoft.EntityFrameworkCore;
using EmployeePortal.Application.Infrastructures;

namespace EmployeePortal.Infrastructure
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly List<ApplicationDbContext> _createdContexts = new();

        public ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IApplicationDbContext CreateDbContext()
        {
            var context = _contextFactory.CreateDbContext();
            _createdContexts.Add(context);
            return context;
        }

       
    }
}