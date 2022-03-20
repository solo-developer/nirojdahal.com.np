using Personal.Domain.Enums;
using System.Collections.Generic;

namespace niroj.website.Areas.UserManagement.Models
{
    public class RolePermissionSaveViewModel
    {
        public string RoleId { get; set; }
        public List<PermissionOptions> Permissions { get; set; } = new List<PermissionOptions>();
    }
}
