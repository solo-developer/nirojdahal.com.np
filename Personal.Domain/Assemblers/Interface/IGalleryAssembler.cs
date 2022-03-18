using Personal.Domain.Dto;
using Personal.Domain.Entities;

namespace Personal.Domain.Assemblers.Interface
{
    public interface IGalleryAssembler
    {
        void Copy(GalleryDto dto, Gallery entity);
    }
}
