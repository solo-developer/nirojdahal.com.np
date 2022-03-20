using Microsoft.AspNetCore.Identity;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool AreUsersPresentInRole(string role_name)
        {
            var userCount = (from user in _context.Users
                             join userRole in _context.UserRoles
                             on user.Id equals userRole.UserId
                             join role in _context.Roles
                             on userRole.RoleId equals role.Id
                             where role.Name == role_name
                             select user)
                                  .Count();

            return userCount > 0;

        }

        public IQueryable<IdentityRole> GetQueryable()
        {
            return _context.Roles.AsQueryable();
        }
    }
}
