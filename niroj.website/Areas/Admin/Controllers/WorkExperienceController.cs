using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Controllers;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/work-experience")]
    public class WorkExperienceController : BaseController
    {
        private readonly IWorkExperienceService _workExperienceService;
        public WorkExperienceController(IWorkExperienceService workExperienceService)
        {
            _workExperienceService = workExperienceService;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var datas = await _workExperienceService.GetAll();
                return View(datas);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception)
            {
                AlertHelper.setMessage(this, "Failed to get work experiences.", MessageType.error);
            }
            return View(new List<WorkExperienceDto>());
        }

        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View(new WorkExperienceDto());
        }

        [Route("save")]
        [HttpPost]
        public async Task<IActionResult> Save(WorkExperienceDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _workExperienceService.Save(dto);
                    AlertHelper.setMessage(this, "Work Experience Saved Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save work experience.", MessageType.error);
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var blog = await _workExperienceService.GetById(id);
                return View("New", blog);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(WorkExperienceDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _workExperienceService.Update(dto);
                    AlertHelper.setMessage(this, "Work Experience updated Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to update work experience.", MessageType.error);
            }
            return View("New", dto);
        }


        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _workExperienceService.Delete(id);
                AlertHelper.setMessage(this, "Experience deleted successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
