using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class ResumeSkillCategoryService : IResumeSkillCategoryService
    {
        private readonly IBaseRepository<ResumeSkillCategory> _resumeSkillCategoryRepo;

        public ResumeSkillCategoryService(IBaseRepository<ResumeSkillCategory> resumeSkillCategoryRepo)
        {
            _resumeSkillCategoryRepo = resumeSkillCategoryRepo;
        }

        public async Task Delete(long id)
        {
            var category = await _resumeSkillCategoryRepo.GetByIdAsync(id) ?? throw new ItemNotFoundException("Skill Category doesnot exist.");
            category.Delete();
            await _resumeSkillCategoryRepo.UpdateAsync(category, id);
        }

        public async Task<List<ResumeSkillCategoryDto>> GetAllUndeleted()
        {
            var categories = await _resumeSkillCategoryRepo.FindAllAsync(a => !a.IsDeleted);

            List<ResumeSkillCategoryDto> response = new List<ResumeSkillCategoryDto>();
            categories.ForEach(cat =>
            {
                response.Add(new ResumeSkillCategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Order = cat.Order,
                });
            });

            return response;
        }

        public async Task<bool> IsNameDuplicate(ResumeSkillCategoryDto dto)
        {
            var catWithSameName = await _resumeSkillCategoryRepo.FindAsync(a => !a.IsDeleted && a.Name.ToLower().Trim().Equals(dto.Name.ToLower().Trim()));

            if (catWithSameName == null)
                return false;

            return catWithSameName.Id != dto.Id;
        }

        public async Task Save(ResumeSkillCategoryDto dto)
        {
            bool isNameDuplicate = await this.IsNameDuplicate(dto);
            if (isNameDuplicate)
                throw new DuplicateItemException("Skill Category with same name already exists.");

            var entity = new ResumeSkillCategory();
            Copy(entity, dto);
            await _resumeSkillCategoryRepo.InsertAsync(entity);
        }

        public async Task Update(ResumeSkillCategoryDto dto)
        {
            var tag = await _resumeSkillCategoryRepo.GetByIdAsync(dto.Id) ?? throw new ItemNotFoundException("Skill Category doesnot exist.");

            bool isNameDuplicate = await this.IsNameDuplicate(dto);
            if (isNameDuplicate)
                throw new DuplicateItemException("Skill Category with same name already exists.");
            Copy(tag, dto);
            await _resumeSkillCategoryRepo.UpdateAsync(tag, dto.Id);
        }

        private void Copy(ResumeSkillCategory entity, ResumeSkillCategoryDto dto)
        {
            entity.Name = dto.Name;
            entity.Order = dto.Order;
        }
    }
}
