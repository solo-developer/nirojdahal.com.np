using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly IContactUsService _contactUsService;
        public ContactController(IContactUsService contactUsService)
        {
            _contactUsService=contactUsService;
        }

        [HttpPost]
        [Route("send-message")]
        public async Task<IActionResult> SendMessage([FromBody]ContactUsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                    return Json(JsonWrapper.buildErrorJson(allErrors.First()));
                }
                bool IsCaptchaValid = await ReCaptchaClass.Validate(dto.Recaptcha) == "true" ? true : false;

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
