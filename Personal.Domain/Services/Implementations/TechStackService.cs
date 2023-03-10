using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class TechStackService : ITechStackService
    {
        private readonly IBaseRepository<SkillCategory> _skillCategoryRepo;
        public TechStackService(IBaseRepository<SkillCategory> skillCategoryRepo)
        {
            _skillCategoryRepo = skillCategoryRepo;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _skillCategoryRepo.GetByIdAsync(id) ?? throw new ItemNotFoundException("Tech stack not found");
            await _skillCategoryRepo.DeleteAsync(entity);

        }

        public async Task<TechStackDto> FindByIdAsync(long id)
        {
            var data = await _skillCategoryRepo.GetByIdAsync(id);
            return new TechStackDto
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                IconsUnformatted = data.Icons,
                Skills = data.Skills.Select(b => b.Name).ToList()
            };
        }

        public async Task<List<TechStackDto>> GetAllAsync()
        {
            var datas = _skillCategoryRepo.GetAllIncluding(a => a.Skills).Select(a => new TechStackDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                IconsUnformatted = a.Icons,
                Skills = a.Skills.Select(b => b.Name).ToList()
            }).ToList();
            return datas;
        }

        public async Task SaveAsync(TechStackDto skill)
        {
            var entity = new SkillCategory(skill.Title, skill.Icons, skill.Skills);
            entity.Description = skill.Description;
            await _skillCategoryRepo.InsertAsync(entity);
        }

        public async Task UpdateAsync(TechStackDto stack)
        {
            var entity = await _skillCategoryRepo.GetByIdAsync(stack.Id) ?? throw new ItemNotFoundException("Tech stack not found");
            entity.Description = stack.Description ?? string.Empty;
            entity.Skills.RemoveAll(a => true);
            foreach (var skill in stack.Skills)
            {
                entity.Skills.Add(new Skill(skill));
            }
            entity.SetIcons(stack.Icons);
            await _skillCategoryRepo.UpdateAsync(entity, entity.Id);
        }
    }
}
