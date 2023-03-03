using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IBaseRepository<Project> _projectRepo;
        public ProjectService(IBaseRepository<Project> projectRepo)
        {
            _projectRepo = projectRepo;
        }
        public async Task Delete(long id)
        {
            var skill = await _projectRepo.GetByIdAsync(id) ?? throw new ItemNotFoundException("Project doesnot exist.");

            await _projectRepo.DeleteAsync(skill);
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var projects = (await _projectRepo.GetAllAsync()).Select(a => new ProjectDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                ProjectType = a.Type
            }).ToList();
            return projects;
        }

        public async Task<ProjectDto> GetByIdAsync(long id)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                ProjectType = project.Type,
                Description = project.Description,
            };
        }

        public async Task Save(ProjectDto dto)
        {
            var entity = new Project();
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Type = dto.ProjectType;
            await _projectRepo.InsertAsync(entity);
        }

        public async Task Update(ProjectDto dto)
        {
            var entity = await _projectRepo.GetByIdAsync(dto.Id) ?? throw new ItemNotFoundException("Project doesnot exist.");
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Type = dto.ProjectType;
            await _projectRepo.UpdateAsync(entity, dto.Id);
        }
    }
}
