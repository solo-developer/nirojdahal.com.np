using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Implementations;
using Personal.Domain.Services.Interface;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            return View(projects);
        }

        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            try
            {
                return View(new ProjectDto());
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
        public async Task<IActionResult> Save(ProjectDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _projectService.Save(dto);
                    AlertHelper.setMessage(this, "Project Saved Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save project detail.", MessageType.error);
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);
                return View("New", project);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(ProjectDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _projectService.Update(dto);
                    AlertHelper.setMessage(this, "Project updated Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to update project.", MessageType.error);
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _projectService.Delete(id);
                AlertHelper.setMessage(this, "Project deleted successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
