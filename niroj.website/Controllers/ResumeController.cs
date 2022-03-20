using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Repository.Interface;

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
        public IActionResult Index()
        {
            //var settings = _settingService.
            return View();
        }
    }
}
