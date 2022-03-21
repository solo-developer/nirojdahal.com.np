using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class ResumeSkillService : IResumeSkillService
    {
        private readonly IBaseRepository<ResumeSkill> _repo;
        private readonly IBaseRepository<ResumeSkillCategory> _categoryRepo;
        public ResumeSkillService(IBaseRepository<ResumeSkill> repo, IBaseRepository<ResumeSkillCategory> categoryRepo)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
        }

        public async Task Delete(long id)
        {
            var skill = await _repo.GetByIdAsync(id) ?? throw new ItemNotFoundException("Skill doesnot exist.");

            await _repo.DeleteAsync(skill);
        }

        public async Task Save(ResumeSkillDto dto)
        {
            var entity = new ResumeSkill();
            entity.Name = dto.Name;
            entity.Category = await _categoryRepo.FindAsync(a => a.Id == dto.CategoryId);
            await _repo.InsertAsync(entity);
        }

        public async Task Update(ResumeSkillDto dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id) ?? throw new ItemNotFoundException("Skill doesnot exist.");
            entity.Name = dto.Name;
            entity.Category = await _categoryRepo.FindAsync(a => a.Id == dto.CategoryId);
            await _repo.UpdateAsync(entity,dto.Id);
        }
    }
}
