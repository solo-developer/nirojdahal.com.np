using Personal.Domain.Entities;
using Personal.Domain.Enums;

namespace Personal.Domain.Repository.Interface
{
    public interface ISettingRepository:IBaseRepository<AppSetting>
    {
        AppSetting GetByKey(AppSettingKeys key);
    }
}
