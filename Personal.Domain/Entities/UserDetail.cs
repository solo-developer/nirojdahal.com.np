using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Domain.Entities
{
    public class UserDetail : BaseEntity
    {
        public long? AddressId { get; set; }
        [Required]
        public string IdentityUserId { get; set; }

        [MaxLength(75)]
        public string ImageName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(15)]
        public string MobileNo { get; set; }

        [MaxLength(75)]
        public string Address { get; set; }


        [ForeignKey(nameof(IdentityUserId))]
        public virtual ApplicationUser AuthenticationDetail { get; set; }

    }
}
