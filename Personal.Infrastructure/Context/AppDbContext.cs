using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Personal.Domain.Entities;
using System.Reflection;

namespace Personal.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
     
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<ContactUs> Contact { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<LastRead> LeadRead { get; set; }
        public DbSet<SkillCategory> SkillCategories { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }
    }
}
