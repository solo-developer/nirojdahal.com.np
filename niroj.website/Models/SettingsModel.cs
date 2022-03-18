using Personal.Domain.Enums;
using System.Collections.Generic;

namespace niroj.website.Models
{
    public class SettingsModel
    {
        public static IReadOnlyDictionary<string, AppSettingKeys> GetKeys()
        {
            return new Dictionary<string, AppSettingKeys>()
            {
                { GetNameKey,AppSettingKeys.Name },
                { GetPositionKey,AppSettingKeys.Position },
                { GetSidebarBio,AppSettingKeys.SidebarBio },
                { GetContentBio,AppSettingKeys.ContentBio },
            };
        }
        public static string GetNameKey { get; } = "Name";
        public static string GetPositionKey { get; } = "Professional Position";
        public static string GetSidebarBio { get; } = "Sidebar Bio";
        public static string GetContentBio { get; } = "Content Bio";
    }
}
