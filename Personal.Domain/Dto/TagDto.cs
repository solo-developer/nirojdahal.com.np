using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class TagDto
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
