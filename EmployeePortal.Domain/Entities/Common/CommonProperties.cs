using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortal.Domain.Entities
{
    public class CommonProperties : BaseEntity, IBaseEntity
    {
        [Column("CreatedByAppUserId")]
        public int CreatedByAppUserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

    }

    public interface IBaseEntity
    {
        bool IsDeleted { get; set; }
    }
}
