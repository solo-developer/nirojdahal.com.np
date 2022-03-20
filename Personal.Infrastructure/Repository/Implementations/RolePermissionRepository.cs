using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly AppDbContext _context;
        public RolePermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Delete(string roleId, List<string> claims)
        {
            var roleClaims = _context.RoleClaims.Where(a => a.RoleId == roleId && claims.Contains(a.ClaimValue)).ToList();

            _context.RoleClaims.RemoveRange(roleClaims);
            _context.SaveChanges();
        }

        public List<string> GetPermissionsOfRole(string RoleId)
        {
            return _context.RoleClaims.Where(a => a.RoleId == RoleId).Select(a=>a.ClaimValue).ToList();
        }

        public void Save(string roleId, List<string> claims)
        {

            foreach (var claim in claims)
            {
                _context.RoleClaims.Add(new Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>()
                {
                    RoleId = roleId,
                    ClaimType = ClaimTypes.AuthorizationDecision,
                    ClaimValue = claim
                });
            }
            _context.SaveChanges();
        }
    }
}
