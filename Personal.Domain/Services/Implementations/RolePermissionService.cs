using Personal.Domain.Enums;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Personal.Domain.Services.Implementations
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepo;
        public RolePermissionService(IRolePermissionRepository rolePermissionRepo)
        {
            _rolePermissionRepo = rolePermissionRepo;
        }

        public void SaveOrUpdate(string roleId, List<PermissionOptions> permissions)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var claims = _rolePermissionRepo.GetPermissionsOfRole(roleId);
                _rolePermissionRepo.Delete(roleId, claims);
                _rolePermissionRepo.Save(roleId, permissions.Select(a => a.ToString()).ToList());
                tx.Complete();
            }
        }
    }
}
