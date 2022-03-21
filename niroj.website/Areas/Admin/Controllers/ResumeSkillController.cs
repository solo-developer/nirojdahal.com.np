using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/resume-skill")]
    public class ResumeSkillController : Controller
    {
        private readonly IResumeSkillService _resumeSkillService;
        private readonly IBaseRepository<ResumeSkill> _resumeSkillRepo;
        private readonly IResumeSkillCategoryService _resumeSkillCategoryService;

        public ResumeSkillController(IResumeSkillService resumeSkillService, IBaseRepository<ResumeSkill> resumeSkillRepo, IResumeSkillCategoryService resumeSkillCategoryService)
        {
            _resumeSkillService = resumeSkillService;
            _resumeSkillRepo = resumeSkillRepo;
            _resumeSkillCategoryService = resumeSkillCategoryService;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var datas = await _resumeSkillRepo.GetAllAsync();

                var dtos = datas.Select(a => new ResumeSkillDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    CategoryName= a.Category.Name
                }).ToList();
                return View(dtos);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception)
            {
                AlertHelper.setMessage(this, "Failed to get skills.", MessageType.error);
            }
            return View(new List<ResumeSkillDto>());
        }

        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            try
            {
                var categories = await _resumeSkillCategoryService.GetAllUndeleted();
                ViewBag.categories = categories;
                return View(new ResumeSkillDto());
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception)
            {
                AlertHelper.setMessage(this, "Failed to get view.", MessageType.error);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("save")]
        [HttpPost]
        public async Task<IActionResult> Save(ResumeSkillDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _resumeSkillService.Save(dto);
                    AlertHelper.setMessage(this, "Skill Saved Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save skill.", MessageType.error);
            }
            finally
            {
                var categories = await _resumeSkillCategoryService.GetAllUndeleted();
                ViewBag.categories = categories;
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("delete/{skill_id}")]
        public async Task<IActionResult> Delete(long skill_id)
        {
            try
            {
                await _resumeSkillService.Delete(skill_id);
                AlertHelper.setMessage(this, "Resume deleted successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
