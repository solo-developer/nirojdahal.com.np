using System.Collections.Generic;

namespace niroj.website.Areas.UserManagement.Models
{
    public class PermissionTreeViewModel
    {
        public bool IsHeader { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public List<PermissionTreeViewModel> Nodes { get; set; } = new List<PermissionTreeViewModel>();
    }
}
