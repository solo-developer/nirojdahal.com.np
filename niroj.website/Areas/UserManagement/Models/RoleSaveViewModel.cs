using System.ComponentModel.DataAnnotations;

namespace niroj.website.Areas.UserManagement.Models
{
    public class RoleSaveViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Role Name is required.")]
        public string Name { get; set; }
    }
}
