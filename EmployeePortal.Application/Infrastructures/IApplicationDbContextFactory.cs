namespace EmployeePortal.Application.Infrastructures
{
    public interface IApplicationDbContextFactory 
    {
        public IApplicationDbContext CreateDbContext();
    }
}
