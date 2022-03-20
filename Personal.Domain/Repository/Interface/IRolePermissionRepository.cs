using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Personal.Domain.Repository.Interface
{
    public interface IRolePermissionRepository
    {
        List<string> GetPermissionsOfRole(string RoleId);
        void Save(string roleId, List<string> claims);
        void Delete(string roleId, List<string> claims);
    }
}
