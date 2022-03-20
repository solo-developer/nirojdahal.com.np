using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Dto;
using Personal.Domain.Enums;
using Personal.Domain.Repository.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("resume")]
    public class ResumeController : Controller
    {
        private readonly ISettingRepository _settingRepo;
        public ResumeController(ISettingRepository settingRepo)
        {
            _settingRepo = settingRepo;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var settings = await _settingRepo.GetAllAsync();
            ViewBag.settings = settings.Select(a => new SettingDto
            {
                Key = (AppSettingKeys)Enum.Parse(typeof(AppSettingKeys), a.Key),
                Value= a.Value
            }).ToList();
            return View();
        }
    }
}
