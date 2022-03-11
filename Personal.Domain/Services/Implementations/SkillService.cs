using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly IBaseRepository<SkillCategory> _skillCategoryRepo;
        public SkillService(IBaseRepository<SkillCategory> skillCategoryRepo)
        {
            _skillCategoryRepo = skillCategoryRepo;
        }

        public async Task<List<SkillCategoryDto>> GetSkills()
        {
            var datas = _skillCategoryRepo.GetAllIncluding(a => a.Skills).Select(a => new SkillCategoryDto
            {
                Title= a.Title,
                Description=a.Description,
                Icons=a.GetIcons(),
                Skills=a.Skills.Select(b=>b.Name).ToArray()
            }).ToList();
            return datas;
        }
    }
}
