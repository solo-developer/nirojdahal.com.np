using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Extensions;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/resume-skill-category")]
    public class ResumeSkillCategoryController : Controller
    {
        private readonly IResumeSkillCategoryService _skillCategoryService;
        private readonly IBaseRepository<ResumeSkillCategory> _skillCategoryRepo;
        private readonly ILog _logService;

        public ResumeSkillCategoryController(IResumeSkillCategoryService skillCatService, IBaseRepository<ResumeSkillCategory> repo, ILog logService)
        {
            _skillCategoryService = skillCatService;
            _skillCategoryRepo = repo;
            _logService = logService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _skillCategoryService.GetAllUndeleted();
            return View(categories);
        }

        [HttpGet]
        [Route("save-update-view")]
        public async Task<IActionResult> SaveOrUpdateView([FromQuery] int id)
        {
            var category = await _skillCategoryRepo.FindAsync(a => a.Id == id);
            var dto = new ResumeSkillCategoryDto();
            if (category != null)
            {
                dto = new ResumeSkillCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Order = category.Order,
                };
            }

            var viewHtml = this.RenderViewAsync("~/Areas/Admin/Views/Partial/_SkillCategoryAddUpdateView.cshtml", dto, true).GetAwaiter().GetResult();

            return Json(JsonWrapper.buildSuccessJson(viewHtml));
        }

        [HttpGet]
        [Route("check-duplicate-name")]
        public async Task<IActionResult> CheckDuplicateName(ResumeSkillCategoryDto model)
        {
            bool isDuplicate = await _skillCategoryService.IsNameDuplicate(model);

            return Json(JsonWrapper.buildSuccessJson(isDuplicate));
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveOrUpdate(ResumeSkillCategoryDto model)
        {
            string errorMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    await _skillCategoryService.Save(model);

                    return Json(JsonWrapper.buildSuccessJson(new { success = true }));
                }
                errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            }
            catch (CustomException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Failed to save/update Skill Category";
                _logService.Error($"Failed to save/update skill category , {ex}");
            }
            return Json(JsonWrapper.buildErrorJson(errorMessage));
        }


        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _skillCategoryService.Delete(id);

                    AlertHelper.setMessage(this, "Skill Category deleted successfully");
                    return RedirectToAction("index");
                }
                AlertHelper.setMessage(this, "Failed to delete Skill Category.", MessageType.error);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to delete skill category , {ex}");
                AlertHelper.setMessage(this, "Failed to delete Skill Category.", MessageType.error);
            }
            return RedirectToAction("index");
        }
    }
}
