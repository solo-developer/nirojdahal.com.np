using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface ISkillService
    {
        Task<List<SkillCategoryDto>> GetSkills();
    }
}
