using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class ContactUsRepository : BaseRepository<ContactUs>, IContactUsRepository
    {
        private readonly AppDbContext _context;
        public ContactUsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
