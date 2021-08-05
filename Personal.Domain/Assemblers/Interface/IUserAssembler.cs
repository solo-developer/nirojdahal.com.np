using Personal.Domain.Dto;
using Personal.Domain.Entities;

namespace Personal.Domain.Assemblers.Interface
{
    public interface IUserAssembler
    {
        void Copy(UserSaveDto dto, UserDetail entity);
    }
}
