using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        private readonly AppDbContext _context;
        public PermissionRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
    }
}
