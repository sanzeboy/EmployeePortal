using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Domain.Entities
{
    /// <summary>
    /// Column configuration for the common property of the column
    /// Inheriet this class and call ConfigureCommon(builder) to add configuration for common properties across all the table
    /// </summary>
    public static class CommonPropertiesConfiguration
    {
        public static void ConfigureCommonProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : CommonProperties
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);
            builder.Property(e => e.CreatedByAppUserId)
                .IsRequired()
                ;

            builder.Property(e => e.CreatedOn)
               .IsRequired()
               .HasDefaultValueSql("getdate()");

            builder.Property(e => e.ModifiedBy)
                .IsRequired(false)
                .HasDefaultValue(null);

            builder.Property(e => e.ModifiedOn)
                .IsRequired(false)
                .HasDefaultValue(null);

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.IsDeleted);


        }
    }
}
