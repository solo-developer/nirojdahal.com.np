using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Personal.Domain.Services.Implementations
{
    public sealed class GalleryService : IGalleryService
    {
        private readonly IGalleryRepository _galleryRepo;
        private readonly IGalleryAssembler _galleryAssembler;
        private readonly IFileHelper _fileHelper;

        private const string IMAGE_FOLDER = "uploads/gallery-img";

        public GalleryService(IGalleryRepository galleryRepo, IGalleryAssembler galleryAssembler, IFileHelper fileHelper)
        {
            _galleryRepo = galleryRepo;
            _galleryAssembler = galleryAssembler;
            _fileHelper = fileHelper;
        }

        public void Delete(long id)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var gallery = _galleryRepo.GetById(id) ?? throw new ItemNotFoundException($"Gallery with id {id} doesn't exist.");

                string oldImage = gallery.ImageName;
                _galleryRepo.Delete(gallery);

                _fileHelper.DeleteFile(IMAGE_FOLDER, oldImage);
                tx.Complete();
            }
        }

        public void Disable(long id)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var gallery = _galleryRepo.GetById(id) ?? throw new ItemNotFoundException($"Image with id {id} doesnot exist.");

                gallery.Disable();
                _galleryRepo.Update(gallery);
                tx.Complete();
            }
        }

        public void Enable(long id)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var gallery = _galleryRepo.GetById(id) ?? throw new ItemNotFoundException($"Image with id {id} doesnot exist.");

                gallery.Enable();
                _galleryRepo.Update(gallery);
                tx.Complete();
            }
        }

        public void MakeSliderImage(long id)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var gallery = _galleryRepo.GetById(id) ?? throw new ItemNotFoundException($"Image with id {id} doesnot exist.");

                gallery.MarkSliderImage();
                _galleryRepo.Update(gallery);
                tx.Complete();
            }
        }

        public void RemoveFromSliderImage(long id)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var gallery = _galleryRepo.GetById(id) ?? throw new ItemNotFoundException($"Image with id {id} doesnot exist.");

                gallery.RemoveFromSliderImage();
                _galleryRepo.Update(gallery);
                tx.Complete();
            }
        }

        public void Save(GalleryDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                Gallery gallery = new Gallery();
                _galleryAssembler.Copy(dto, gallery);
                _galleryRepo.Insert(gallery);
                _fileHelper.MoveImageFromTempPathToDestination(dto.ImageName, IMAGE_FOLDER);
                tx.Complete();
            }
        }

        public void Update(GalleryDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var gallery = _galleryRepo.GetById(dto.Id) ?? throw new ItemNotFoundException("Gallery image with specified detail doesnot exist.");


                string oldImage = gallery.ImageName;

                _galleryAssembler.Copy(dto, gallery);
                _galleryRepo.Update(gallery);

                if (!string.IsNullOrWhiteSpace(dto.ImageName))
                {
                    _fileHelper.MoveImageFromTempPathToDestination(dto.ImageName, IMAGE_FOLDER);
                    if (!string.IsNullOrWhiteSpace(oldImage))
                    {
                        _fileHelper.DeleteFile(IMAGE_FOLDER, oldImage);
                    }
                }
                tx.Complete();
            }
        }

        public async Task<List<GalleryDto>> GetAllAsync()
        {
            var images =await _galleryRepo.GetAllAsync();
            List<GalleryDto> response = new List<GalleryDto>();
            foreach (var image in images)
            {
                GalleryDto dto = new GalleryDto();
                CopyToDto(image, dto);
                response.Add(dto);
            }
            return response;
        }


        public async Task<List<GalleryDto>> GetBannerImages()
        {
            var bannerImages =await _galleryRepo.FindAllAsync(a => a.IsSliderImage && a.IsEnabled);

            List<GalleryDto> response = new List<GalleryDto>();
            foreach (var image in bannerImages)
            {
                GalleryDto dto = new GalleryDto();
                CopyToDto(image, dto);
                response.Add(dto);
            }
            return response;
        }
        public GalleryDto GetById(long id)
        {
            var image = _galleryRepo.GetById(id) ?? throw new ItemNotFoundException("Gallery with specified detail doesnot exist.");
            GalleryDto dto = new GalleryDto();
            CopyToDto(image, dto);
            return dto;
        }

        private void CopyToDto(Gallery image, GalleryDto dto)
        {
            dto.Id = image.Id;
            dto.ImageName = image.ImageName;
            dto.Description = image.Description;
            dto.Title = image.Title;
            dto.IsSliderImage = image.IsSliderImage;
            dto.IsEnabled = image.IsEnabled;
        }
    }
}
