using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Personal.Domain.Repository.Interface
{
    public interface IRoleRepository
    {
        IQueryable<IdentityRole> GetQueryable();
        bool AreUsersPresentInRole(string role_name);
    }
}
