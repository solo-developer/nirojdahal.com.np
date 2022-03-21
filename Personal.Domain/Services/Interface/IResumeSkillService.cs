using Personal.Domain.Dto;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IResumeSkillService
    {
        Task Save(ResumeSkillDto dto);
        Task Update(ResumeSkillDto dto);

        Task Delete(long id);
    }
}
