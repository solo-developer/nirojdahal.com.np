using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using Personal.Domain.Configs;
using Personal.Domain.Dto;
using Personal.Domain.Enums;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly IContactUsService _contactUsService;
        private readonly RecaptchaConfiguration _recaptchaConfiguration;
        private readonly ISettingRepository _settingRepo;

        public ContactController(IContactUsService contactUsService, RecaptchaConfiguration recaptchaConfig, ISettingRepository settingRepo)
        {
            _contactUsService = contactUsService;
            _recaptchaConfiguration = recaptchaConfig;
            _settingRepo = settingRepo;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var entities = await _settingRepo.GetAllAsync();
            var settings = entities.Select(a => new SettingDto
            {
                Key = (AppSettingKeys)Enum.Parse(typeof(AppSettingKeys), a.Key),
                Value = a.Value
            }).ToList();

            return View(settings);
        }

        [HttpPost]
        [Route("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] ContactUsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                    return Json(JsonWrapper.buildErrorJson(allErrors.First()));
                }
                bool IsCaptchaValid = await ReCaptchaClass.Validate(_recaptchaConfiguration, dto.Recaptcha) == "true" ? true : false;

                if (IsCaptchaValid)
                {
                    _contactUsService.Save(dto);
                    return Json(JsonWrapper.buildSuccessJson(true));
                }

                throw new CustomException("Recaptcha is not valid.");
            }
            catch (CustomException ex)
            {
                return Json(JsonWrapper.buildErrorJson(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(JsonWrapper.buildErrorJson("Failed to send message."));
            }
        }
    }
}
