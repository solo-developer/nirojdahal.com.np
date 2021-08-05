using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class BlogRepository : BaseRepository<Blog>, IBlogRepository
    {
        private readonly AppDbContext _context;
        public BlogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
