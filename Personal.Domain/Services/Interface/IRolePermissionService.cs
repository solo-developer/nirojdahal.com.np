using Personal.Domain.Enums;
using System.Collections.Generic;

namespace Personal.Domain.Services.Interface
{
    public interface IRolePermissionService
    {
        void SaveOrUpdate(string roleId, List<PermissionOptions> permissions);
    }
}
