using Personal.Domain.Enums;
using System.Collections.Generic;

namespace niroj.website.Areas.Admin.Models
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
                { GetPhone,AppSettingKeys.Phone },
                { GetEmail,AppSettingKeys.Email },
                { GetWebsite,AppSettingKeys.Website },
                { GetLocation,AppSettingKeys.Location },
                { GetResumeSumary,AppSettingKeys.ResumeSummary },
                { GetGithubLink,AppSettingKeys.GithubLink },
                { GetLinkedInLink,AppSettingKeys.LinkedInLink },
                { GetTwitterLink,AppSettingKeys.TwitterLink },
            };
        }
        public static string GetNameKey { get; } = "Name";
        public static string GetPositionKey { get; } = "Professional Position";
        public static string GetSidebarBio { get; } = "Sidebar Bio";
        public static string GetContentBio { get; } = "Content Bio";
        public static string GetPhone { get; } = "Phone";
        public static string GetEmail { get; } = "Email";
        public static string GetWebsite { get; } = "Website";
        public static string GetLocation { get; } = "Location";
        public static string GetResumeSumary { get; } = "Resume Summary";
        public static string GetGithubLink { get; } = "Github URL";
        public static string GetLinkedInLink { get; } = "LinkedIn URL";
        public static string GetTwitterLink { get; } = "Twitter URL";
    }
}
