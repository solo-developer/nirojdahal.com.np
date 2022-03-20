using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IWorkExperienceService
    {
        Task<List<WorkExperienceDto>> GetAll();
        Task<WorkExperienceDto> GetById(long id);
        Task Save(WorkExperienceDto dto);
        Task Update(WorkExperienceDto dto);
        Task Delete(long id);
    }
}
