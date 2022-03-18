using Personal.Domain.Enums;

namespace Personal.Domain.Dto
{
    public class SettingDto
    {
        public AppSettingKeys Key { get; set; }

        public string Value { get; set; }
    }
}
