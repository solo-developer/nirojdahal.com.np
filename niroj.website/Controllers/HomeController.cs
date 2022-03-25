using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseRepository<AppSetting> _appSettingRepo;
        public HomeController(IBaseRepository<AppSetting> appSettingRepo)
        {
            _appSettingRepo = appSettingRepo;
        }

        public async Task<IActionResult> Index()
        {
            throw new InvalidOperationException();
            var appSettings = await _appSettingRepo.GetAllAsync();
            Dictionary<string, string> responseData = new Dictionary<string, string>();
            appSettings.ForEach(appSetting => responseData.Add(appSetting.Key, appSetting.Value));
            return View(responseData);
        }
    }
}
