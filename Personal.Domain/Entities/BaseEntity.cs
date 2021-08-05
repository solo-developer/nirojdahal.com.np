using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
