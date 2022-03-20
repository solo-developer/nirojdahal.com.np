using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Personal.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public long ModuleId { get; set; }
        public long? ParentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string DisplayName { get; set; }

        [Required]
        public bool IsHeader { get; set; } = false;

        [ForeignKey(nameof(ParentId))]
        public virtual Permission ParentPermission { get; set; }

        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }
        public virtual List<Permission> ChildPermissions { get; set; } = new List<Permission>();

        public bool HasChildren() => ChildPermissions.Any();
    }
}
