using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class WorkExperienceService : IWorkExperienceService
    {
        private readonly IBaseRepository<WorkExperience> _repo;
        public WorkExperienceService(IBaseRepository<WorkExperience> repo)
        {
            _repo = repo;
        }

        public async Task Delete(long id)
        {
            var entity = await _repo.FindAsync(a => a.Id == id) ?? throw new ItemNotFoundException("Experience not found.");

            await _repo.DeleteAsync(entity);
        }

        public async Task<List<WorkExperienceDto>> GetAll()
        {
            var data = await _repo.GetAllAsync();

            var response = new List<WorkExperienceDto>();
            data.ForEach(entity =>
            {
                var dto = new WorkExperienceDto();
                Copy(dto,entity);
                response.Add(dto);
            });
            return response;
        }

        public async Task<WorkExperienceDto> GetById(long id)
        {
            var entity = await _repo.FindAsync(a => a.Id == id) ?? throw new ItemNotFoundException("Experience not found.");
            var response = new WorkExperienceDto();
            Copy(response,entity);
            return response;
        }

        public async Task Save(WorkExperienceDto dto)
        {
            var entity = new WorkExperience();
            Copy(entity, dto);
            await _repo.InsertAsync(entity);
        }

        public async Task Update(WorkExperienceDto dto)
        {
            var entity = await _repo.FindAsync(a => a.Id == dto.Id) ?? throw new ItemNotFoundException("Experience not found.");
            Copy(entity, dto);
            await _repo.UpdateAsync(entity, dto.Id);
        }

        private void Copy(WorkExperience entity, WorkExperienceDto dto)
        {
            entity.CompanyName = dto.CompanyName;
            entity.Position = dto.Position;
            entity.StartYear = dto.StartYear;
            entity.EndYear = dto.EndYear;
            entity.RoleDescription = dto.RoleDescription;
        }

        private void Copy(WorkExperienceDto dto, WorkExperience entity)
        {
            dto.Id = entity.Id;
            dto.CompanyName = entity.CompanyName;
            dto.Position = entity.Position;
            dto.StartYear = entity.StartYear;
            dto.EndYear = entity.EndYear;
            dto.RoleDescription = entity.RoleDescription;
        }
    }
}
