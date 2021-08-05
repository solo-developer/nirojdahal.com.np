using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class BlogCategoryRepository : BaseRepository<BlogCategory>,IBlogCategoryRepository
    {
        private readonly AppDbContext _context;
        public BlogCategoryRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
    }
}
