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
        public DbSet<BlogTagMap> BlogTagMap { get; set; }
        public DbSet<ContactUs> Contact { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LastRead> LeadRead { get; set; }
        public DbSet<Newsletter> NewsLetters { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ResumeSkillCategory> ResumeSkillCategories { get; set; }
        public DbSet<ResumeSkill> ResumeSkills { get; set; }
      
        public DbSet<SkillCategory> SkillCategories { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
    }
}
