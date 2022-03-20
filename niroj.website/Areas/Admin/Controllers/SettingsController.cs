using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Enums;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/settings")]
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly ISettingRepository _settingRepo;

        public SettingsController(ISettingService settingService, ISettingRepository settingRepo)
        {
            _settingService = settingService;
            _settingRepo = settingRepo;
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
                    Key =appsettingKey,
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
            }
            return RedirectToAction("index");
        }
    }
}
