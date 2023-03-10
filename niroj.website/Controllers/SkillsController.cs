using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Services.Interface;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    public class SkillsController : Controller
    {
        private readonly ITechStackService _skillService;
        public SkillsController(ITechStackService skillService)
        {
            _skillService = skillService;
        }

        public async Task<IActionResult> Index()
        {
            var skills=await _skillService.GetAllAsync();
            return PartialView("~/Views/Skills/_Skills.cshtml", skills);
        }
    }
}
