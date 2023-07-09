using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Enums;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/settings")]
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly ISettingRepository _settingRepo;
        private readonly ILog _logService;
        private readonly IFileHelper _fileHelper;
        private readonly IWebHostEnvironment _environment;

        public SettingsController(ISettingService settingService, ISettingRepository settingRepo, ILog logService,IFileHelper fileHelper,IWebHostEnvironment environment)
        {
            _settingService = settingService;
            _settingRepo = settingRepo;
            _logService = logService;
            _fileHelper = fileHelper;
            _environment = environment;
        }

        [Route("personal-info")]
        public IActionResult Index()
        {
            var settings = _settingRepo.GetAll();
            List<SettingDto> dtos = new List<SettingDto>();
            foreach (var setting in settings)
            {
                Enum.TryParse<AppSettingKeys>(setting.Key, out var appsettingKey);

                dtos.Add(new SettingDto()
                {
                    Key = appsettingKey,
                    Value = setting.Value
                });
            }
            return View(dtos);
        }

        [Route("personal-info/save")]
        [HttpPost]

        public IActionResult SavePersonalInfo(List<SettingDto> datas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    datas.RemoveAll(a => string.IsNullOrWhiteSpace(a.Value));

                    _settingService.SaveOrUpdate(datas);
                    AlertHelper.setMessage(this, "Personal information saved successfully.");
                    return RedirectToAction("index");
                }
                return View(datas);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save personal information", MessageType.error);
                _logService.Error($"Failed to save personal information, {ex}");
            }
            return RedirectToAction("index");
        }

        [Route("personal-info/upload-resume")]
        [HttpPost]

        public async Task<IActionResult> ResumeUpload(ResumeUploadViewModel model)
        {
            try
            {
                if (model==null || model.Resume==null)
                {
                    AlertHelper.setMessage(this, "Please select a file",MessageType.error);
                    return RedirectToAction("index");
                }
                _fileHelper.DeleteFile($@"{_environment.WebRootPath}/attachments", "Niroj_Dahal_Resume.pdf");

                var filePath = Path.Combine($@"{_environment.WebRootPath}/attachments", "Niroj_Dahal_Resume.pdf");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Resume.CopyToAsync(stream);
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to upload resume", MessageType.error);
                _logService.Error($"Failed to upload resume, {ex}");
            }
            return RedirectToAction("index");
        }

        public class ResumeUploadViewModel
        {
            public IFormFile Resume { get; set; }
        }
    }
}
