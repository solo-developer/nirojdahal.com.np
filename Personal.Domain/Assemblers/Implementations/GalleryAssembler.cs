using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;

namespace Personal.Domain.Assemblers.Implementations
{
    public class GalleryAssembler : IGalleryAssembler
    {
        public void Copy(GalleryDto dto, Gallery gallery)
        {
            gallery.Id = dto.Id;
            if (!string.IsNullOrWhiteSpace(dto.ImageName))
            {
                gallery.ImageName = dto.ImageName;
            }
            gallery.Title = dto.Title.Trim();
            gallery.Description = dto.Description.Trim();
            gallery.IsEnabled = dto.IsEnabled;
        }
    }
}
