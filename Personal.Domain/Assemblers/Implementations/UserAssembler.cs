using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;

namespace Personal.Domain.Assemblers.Implementations
{
    public class UserAssembler : IUserAssembler
    {
        public void Copy(UserSaveDto dto, UserDetail entity)
        {
            entity.ImageName = dto.ImageName;
            entity.FullName = dto.FullName;
            entity.MobileNo = dto.MobileNo;
        }
    }
}
