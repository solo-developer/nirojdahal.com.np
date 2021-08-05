using Personal.Domain.Dto;
using System.Collections.Generic;

namespace Personal.Domain.Services.Interface
{
    public interface IUserService
    {
        UserDto GetById(long id);

        UserDto GetByAspUserId(string id);

        List<UserDto> GetAll();

        void Save(UserSaveDto dto);
        void Edit(UserSaveDto dto);
    }
}
