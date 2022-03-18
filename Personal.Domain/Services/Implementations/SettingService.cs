using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Transactions;

namespace Personal.Domain.Services.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepo;
        public SettingService(ISettingRepository settingRepo)
        {
            _settingRepo = settingRepo;
        }

        public void SaveOrUpdate(SettingDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var setup = _settingRepo.GetByKey(dto.Key);

                if (setup == null)
                {
                    save(dto);
                }
                else
                {
                    Update(setup, dto.Value);
                }

                tx.Complete();
            }
        }

        public void SaveOrUpdate(List<SettingDto> settings)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                foreach (var setting in settings)
                {
                    SaveOrUpdate(setting);
                }
                tx.Complete();
            }
        }

        private void Update(AppSetting setup, string value)
        {
            setup.Value = value;
            _settingRepo.Update(setup);
        }

        private void save(SettingDto dto)
        {
            AppSetting setup = new AppSetting();
            setup.Key = dto.Key.ToString();
            setup.Value = dto.Value;
            _settingRepo.Insert(setup);
        }
    }
}
