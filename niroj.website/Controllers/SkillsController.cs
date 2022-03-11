using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Services.Interface;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    public class SkillsController : Controller
    {
        private readonly ISkillService _skillService;
        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        public async Task<IActionResult> Index()
        {
            var skills=await _skillService.GetSkills();
            return PartialView("~/Views/Skills/_Skills.cshtml", skills);
        }
    }
}
