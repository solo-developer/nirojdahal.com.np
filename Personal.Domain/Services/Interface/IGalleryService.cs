using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IGalleryService
    {
        void Save(GalleryDto gallery_dto);
        void Update(GalleryDto gallery_dto);
        void Delete(long gallery_id);
        void Enable(long gallery_id);
        void Disable(long gallery_id);
        void MakeSliderImage(long gallery_id);
        void RemoveFromSliderImage(long gallery_id);

        Task<List<GalleryDto>> GetAllAsync();

        Task<List<GalleryDto>> GetBannerImages();

        GalleryDto GetById(long id);
    }
}
