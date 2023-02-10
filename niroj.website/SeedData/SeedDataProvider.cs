using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.SeedData
{
    public static class SeedDataProvider
    {
        private const string DESKTOP_KEY = "Desktop";
        private const string MOBILE_APP_DEVELOPMENT_KEY = "Mobile App Development";
        private const string WEB_DEVELOPMENT_KEY = "Web Development";
        private const string FRAMEWORK_AND_LIBRARIES_KEY = "Framework & Libraries";
        private const string MAJOR_OS_AND_TOOLS_KEY = "Major OS & Tools";
        private const string OTHERS_KEY = "Others";

        private static readonly Dictionary<string, string[]> _skillSets =
            new Dictionary<string, string[]> {
            {DESKTOP_KEY,new string[]{ "Winforms","WPF","Xamarin"} },
            {MOBILE_APP_DEVELOPMENT_KEY,new string[]{ "Xamarin.Android","Xamarin.Forms"} },
            {WEB_DEVELOPMENT_KEY,new string[]{ "ASP.Net Core 2.1,3.0",".NET 5",".NET 6","ASP.NET MVC 5"} },
            {FRAMEWORK_AND_LIBRARIES_KEY,new string[]{ "ASP.NET MVC 5","ASP.NET Web API","Entity Framework","Entity Framework Core","ASP.NET Core","ASP.NET Core Web API","SignalR","Bootstrap","Javascript","JQuery,Angular 7, React JS"} },
            {MAJOR_OS_AND_TOOLS_KEY,new string[]{ "Windows","SQL Server Management Studio","Visual Studio"} },
            {OTHERS_KEY,new string[]{ "Version Control (GIT, TFS)","Unit Testing","Domain-Driven Design (DDD)","ORM (NHibernate,EF Core, EF)","Ajax","LINQ",} },
            };

        private static readonly Dictionary<string, string[]> _skillIcons = new Dictionary<string, string[]> {
            {DESKTOP_KEY,new string[]{"fa fa-desktop"}},
            {MOBILE_APP_DEVELOPMENT_KEY,new string[]{"fa fa-mobile"}},
            {WEB_DEVELOPMENT_KEY,new string[]{"fab fa-chrome"}},
            {FRAMEWORK_AND_LIBRARIES_KEY,new string[]{"fab fa-angular me-2","fab fa-react me-2"}},
            {MAJOR_OS_AND_TOOLS_KEY,new string[]{"fab fa-windows"}},
            {OTHERS_KEY,new string[]{"fab fa-git"}},
            };

        private static readonly Dictionary<string, string> _skillCategoryDescriptions = new Dictionary<string, string>
        {
            {DESKTOP_KEY,"Started Career as desktop software developer in Winforms and continuing the same using WPF framework." },
            {MOBILE_APP_DEVELOPMENT_KEY,"I have been developing mobile app for about a year now with the frameworks mentioned." },
            {WEB_DEVELOPMENT_KEY,"I have been developing web application for more than 4 years now with the frameworks mentioned" }
        };

        private static readonly Dictionary<string, string> _appSettings = new Dictionary<string, string>
        {
            {AppSettingKeys.Name.ToString(),"Niroj Dahal" },
            {AppSettingKeys.Position.ToString(),"Senior Software Engineer" },
            {AppSettingKeys.SidebarBio.ToString(),"Hi, my name is Niroj Dahal and I'm a full-stack developer. Welcome to my personal website!" },
            {AppSettingKeys.ContentBio.ToString()," I'm a software engineer specialised in frontend and backend development for complex scalable web apps. Want to know more about me? Check out my resume." },
        };

        private static string _roleId = Guid.NewGuid().ToString();
        public static async Task SeedUsers(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            var role = context.Roles.Where(a => a.Name == "admin").SingleOrDefault();
            if (role == null)
            {
                role = new IdentityRole()
                {
                    Id = _roleId,
                    Name = "admin",
                    NormalizedName = "admin".ToUpper()
                };

                context.Roles.Add(role);
                await context.SaveChangesAsync();
            }
            _roleId = role.Id;
            if (!context.Users.Any(a => a.UserName == "admin"))
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "developer.niroj@gmail.com"
                };

                IdentityResult result = await userManager.CreateAsync(user, "Pass@word1");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role.Name);

                    context.UserDetails.Add(new UserDetail
                    {
                        IdentityUserId=user.Id,
                        FullName="Niroj Dahal",
                        MobileNo="+977-9816003056",
                        Address="Mahalaxmisthan,Lalitpur",
                        ImageName="my-image.jpg"
                    });
                }
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedSkills(AppDbContext context)
        {
            var skillCategories = context.SkillCategories.AsEnumerable();
            foreach (var key in _skillIcons.Keys)
            {
                if (!skillCategories.Any(a => a.Title.Equals(key)))
                {
                    var description = _skillCategoryDescriptions.ContainsKey(key) ? _skillCategoryDescriptions[key] : string.Empty;
                    context.SkillCategories.Add(
                        new SkillCategory(key, _skillIcons[key].ToList(), _skillSets[key].ToList())
                        {
                            Description = description
                        });
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedAppSettings(AppDbContext context)
        {
            var appSettings = context.AppSettings.AsEnumerable();
            foreach (var key in _appSettings.Keys)
            {
                if (!appSettings.Any(a => a.Key.Equals(key)))
                {
                    context.AppSettings.Add(new AppSetting(key, _appSettings[key]));
                }
            }

            await context.SaveChangesAsync();
        }

        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                SeedUsers(context, userManager).GetAwaiter().GetResult();
                SeedSkills(context).GetAwaiter().GetResult();
                SeedAppSettings(context).GetAwaiter().GetResult();
            }
            return host;
        }
    }
}
