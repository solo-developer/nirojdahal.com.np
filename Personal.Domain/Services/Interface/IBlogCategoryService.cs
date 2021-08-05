using Personal.Domain.Dto;
using System.Collections.Generic;

namespace Personal.Domain.Services.Interface
{
    public interface IBlogCategoryService
    {
        List<BlogCategoryDto> GetAll();
        BlogCategoryDto GetById(long id);
        void Save(BlogCategoryDto dto);
        void Update(BlogCategoryDto dto);
        void Delete(long id);

    }
}
