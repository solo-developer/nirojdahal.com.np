using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Personal.Domain.Dto
{
    public class TechStackDto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public string IconsUnformatted { get; set; } = string.Empty;
        public List<string> Icons =>
             string.IsNullOrEmpty(IconsUnformatted) ? new List<string>() : IconsUnformatted.Split(",").ToList();

    public List<string> Skills { get; set; } = new List<string>();
    }
}
