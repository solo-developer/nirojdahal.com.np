using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface ITagService
    {
        Task Save(TagDto dto);
        Task Update(TagDto dto);

        Task Delete(long id);

        Task<List<TagDto>> GetAllUndeleted();
    }
}
