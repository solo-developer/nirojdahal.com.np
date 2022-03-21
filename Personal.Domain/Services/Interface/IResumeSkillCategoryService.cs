using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IResumeSkillCategoryService
    {
        Task Save(ResumeSkillCategoryDto dto);
        Task Update(ResumeSkillCategoryDto dto);

        Task Delete(long id);

        Task<List<ResumeSkillCategoryDto>> GetAllUndeleted();
        Task<bool> IsNameDuplicate(ResumeSkillCategoryDto dto);
    }
}
