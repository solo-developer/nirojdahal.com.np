using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class AppSetting : BaseEntity
    {
        public AppSetting()
        {

        }
        public AppSetting(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [Required, MaxLength(100)]
        public string Key { get; set; }

        [Required, MaxLength(300)]
        public string Value { get; set; }
    }
}
