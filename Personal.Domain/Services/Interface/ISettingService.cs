using Personal.Domain.Dto;
using System.Collections.Generic;

namespace Personal.Domain.Services.Interface
{
    public interface ISettingService
    {
        void SaveOrUpdate(SettingDto dto);
        void SaveOrUpdate(List<SettingDto> settings);
    }
}
