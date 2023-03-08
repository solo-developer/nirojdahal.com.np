using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Areas.Admin.Models;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Services.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/gallery")]
    public class GalleryController : Controller
    {
        private readonly IGalleryService _galleryService;
        private readonly IFileHelper _fileHelper;
        private readonly ILog _logService;

        public GalleryController(IGalleryService galleryService, IFileHelper fileHelper,ILog logService)
        {
            _galleryService = galleryService;
            _fileHelper = fileHelper;
            _logService = logService;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var images = await _galleryService.GetAllAsync();
            return View(images);
        }

        [HttpGet]
        [Route("new")]
        // [Authorize(Policy = "AddGalleryImage")]
        public IActionResult Add()
        {
            try
            {
                GallerySaveModel galleryModel = new GallerySaveModel();
                return View(galleryModel);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to get add galary view , {ex}");
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("new")]
        //  [Authorize(Policy = "AddGalleryImage")]
        public async Task<IActionResult> Add(GallerySaveModel model)
        {
            try
            {
                if (model.File == null)
                {
                    throw new CustomException("Image is required.");
                }
                if (ModelState.IsValid)
                {
                    GalleryDto galleryDto = new GalleryDto();
                    galleryDto.Title = model.Title;
                    string tempPath = Path.GetTempPath();
                    galleryDto.ImageName = await _fileHelper.SaveImageAndGetFileName(model.File, tempPath, model.Title);

                    galleryDto.Description = model.Description;
                    galleryDto.IsEnabled = model.IsEnabled;
                    galleryDto.IsSliderImage = model.IsSliderImage;
                    _galleryService.Save(galleryDto);

                    AlertHelper.setMessage(this, "Gallery Image saved successfully.", MessageType.success);
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to get save galary item , {ex}");
            }

            return View(model);
        }

        [HttpGet]
        [Route("edit/{id}")]
        // [Authorize(Policy = "UpdateGalleryImage")]
        public IActionResult Edit(long id)
        {
            try
            {
                var image = _galleryService.GetById(id);
                GallerySaveModel galleryModel = new GallerySaveModel()
                {
                    Id = image.Id,
                    ImageName = image.ImageName,
                    Title = image.Title,
                    Description = image.Description,
                    IsEnabled = image.IsEnabled,
                    IsSliderImage = image.IsSliderImage,

                };
                return View("Add", galleryModel);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to get edit galary view , {ex}");
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("edit")]
        // [Authorize(Policy = "UpdateGalleryImage")]
        public async Task<IActionResult> Edit(GallerySaveModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryDto galleryDto = new GalleryDto();
                    galleryDto.Id = model.Id;
                    galleryDto.Title = model.Title;
                    galleryDto.Description = model.Description;
                    if (model.File != null)
                    {
                        var tempPath = Path.GetTempPath();
                        galleryDto.ImageName = await _fileHelper.SaveImageAndGetFileName(model.File, tempPath, model.Title);
                    }

                    galleryDto.IsSliderImage = model.IsSliderImage;
                    galleryDto.IsEnabled = model.IsEnabled;

                    _galleryService.Update(galleryDto);
                    AlertHelper.setMessage(this, "Gallery Image updated successfully.", MessageType.success);
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to edit galary item , {ex}");
            }
            return View(model);
        }

        [HttpGet]
        [Route("enable/{id}")]
        //  [Authorize(Policy = "UpdateGalleryImage")]
        public IActionResult Enable(long id)
        {
            try
            {
                _galleryService.Enable(id);
                AlertHelper.setMessage(this, "Gallery Image enabled successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to enable galary item , {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("disable/{id}")]
        // [Authorize(Policy = "UpdateGalleryImage")]
        public IActionResult Disable(long id)
        {
            try
            {
                _galleryService.Disable(id);
                AlertHelper.setMessage(this, "Gallery Image disabled successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to get disable galary item , {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("make-slider/{id}")]
        //  [Authorize(Policy = "UpdateGalleryImage")]
        public IActionResult MakeSlider(long id)
        {
            try
            {
                _galleryService.MakeSliderImage(id);
                AlertHelper.setMessage(this, "Gallery Image set to Home image slider successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to make slider image , {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("unmake-slider/{id}")]
        // [Authorize(Policy = "UpdateGalleryImage")]
        public IActionResult UnmarkSlider(long id)
        {
            try
            {
                _galleryService.RemoveFromSliderImage(id);
                AlertHelper.setMessage(this, "Gallery Image removed from Home slider image successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to remove item from slider image , {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("delete/{gallery_id}")]
        //   [Authorize(Policy = "DeleteGalleryImage")]
        public IActionResult Delete(long gallery_id)
        {
            try
            {
                _galleryService.Delete(gallery_id);
                AlertHelper.setMessage(this, "Gallery image deleted successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to delete galary item , {ex}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
