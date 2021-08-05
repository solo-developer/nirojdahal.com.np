using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class ApplicationUserRepository:BaseRepository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepository(AppDbContext context):base(context)
        {

        }
    }
}
