using Personal.Domain.Dto;
using Personal.Domain.Entities;

namespace Personal.Domain.Assemblers.Interface
{
    public interface IBlogCategoryAssembler
    {
        void Copy(BlogCategoryDto dto, BlogCategory entity);
    }
}
