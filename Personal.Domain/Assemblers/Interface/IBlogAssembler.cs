using Personal.Domain.Dto;
using Personal.Domain.Entities;

namespace Personal.Domain.Assemblers.Interface
{
    public interface IBlogAssembler
    {
        void Copy(BlogDto dto, Blog entity);
    }
}
