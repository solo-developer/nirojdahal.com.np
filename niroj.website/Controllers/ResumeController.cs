using Microsoft.AspNetCore.Mvc;
using niroj.website.ViewModels;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("resume")]
    public class ResumeController : Controller
    {
        private readonly ISettingRepository _settingRepo;
        private readonly IBaseRepository<ResumeSkillCategory> _skillCategoryRepo;
        private readonly IWorkExperienceService _workExperienceService;

        public ResumeController(ISettingRepository settingRepo, IBaseRepository<ResumeSkillCategory> skillCategoryRepo,IWorkExperienceService workExperienceService)
        {
            _settingRepo = settingRepo;
            _skillCategoryRepo = skillCategoryRepo;
            _workExperienceService = workExperienceService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var response = new ResumeIndexViewModel();
            var settings = await _settingRepo.GetAllAsync();
            response.Settings = settings.Select(a => new SettingDto
            {
                Key = (AppSettingKeys)Enum.Parse(typeof(AppSettingKeys), a.Key),
                Value = a.Value
            }).ToList();

            var skillCategories = await _skillCategoryRepo.FindAllAsync(a => !a.IsDeleted);

            skillCategories.OrderBy(a=>a.Order).ToList().ForEach(skillCategory =>
            {
                response.SkillCategories.Add(new ResumeSkillCategoryViewModel
                {
                    Name = skillCategory.Name,
                    Skills = skillCategory.Skills.Select(b => b.Name).ToList()
                });
            });

            response.Experiences = await _workExperienceService.GetAll();

            return View(response);
        }
    }
}
