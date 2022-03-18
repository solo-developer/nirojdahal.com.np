using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Domain.Repository.Interface;
using Personal.Infrastructure.Context;
using System.Linq;

namespace Personal.Infrastructure.Repository.Implementations
{
    public class SettingRepository : BaseRepository<AppSetting>, ISettingRepository
    {
        private readonly AppDbContext _context;
        public SettingRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public AppSetting GetByKey(AppSettingKeys key)
        {
              return _context.AppSettings.Where(a => a.Key == key.ToString()).SingleOrDefault();
        }
    }
}
