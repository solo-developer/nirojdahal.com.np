using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class GalleryRepository : BaseRepository<Gallery>, IGalleryRepository
    {
        private readonly AppDbContext _context;
        public GalleryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
