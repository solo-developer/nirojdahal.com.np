using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface ITechStackService
    {
        Task<List<TechStackDto>> GetAllAsync();
        Task<TechStackDto> FindByIdAsync(long id);
        Task SaveAsync(TechStackDto stack);
        Task UpdateAsync(TechStackDto stack);
        Task DeleteAsync(long id);
    }
}
