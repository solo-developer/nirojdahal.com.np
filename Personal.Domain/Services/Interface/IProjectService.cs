using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IProjectService
    {
        Task Save(ProjectDto dto);
        Task Update(ProjectDto dto);

        Task<ProjectDto> GetByIdAsync(long id);
        Task<List<ProjectDto>> GetAllAsync();

        Task Delete(long id);
    }
}
