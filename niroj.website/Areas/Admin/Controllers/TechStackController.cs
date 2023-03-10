using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Extensions;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Services.Interface;
using System;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/tech-stack")]
    public class TechStackController : Controller
    {
        private readonly ITechStackService _techStackService;
        private readonly ILog _logService;
        public TechStackController(ITechStackService techStackService, ILog logService)
        {
            _techStackService = techStackService;
            _logService = logService;
        }

        [Route("")]
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _techStackService.GetAllAsync();
            return View(datas);
        }

        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            try
            {
                return View(new TechStackDto());
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to get view.", MessageType.error);
                _logService.Error($"Failed to get add skills view, {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("save")]
        [HttpPost]
        public async Task<IActionResult> Save(TechStackDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _techStackService.SaveAsync(dto);
                    AlertHelper.setMessage(this, "Tech Stack Saved Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save tech stack.", MessageType.error);
                _logService.Error($"Failed to save tech stack, {ex}");
            }
            return View("New", dto);
        }
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var data = await _techStackService.FindByIdAsync(id);

                return View("New", data);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to get edit tech stack view ,{ex}");
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(TechStackDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _techStackService.UpdateAsync(dto);
                    AlertHelper.setMessage(this, "Tech Stack updated Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to update tech stack.", MessageType.error);
                _logService.Error($"Failed to update tech stack ,{ex}");
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _techStackService.DeleteAsync(id);
                AlertHelper.setMessage(this, "Tech Stack deleted successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to delete Tech Stack, {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("new-tech-stack-row")]
        [HttpGet]
        public async Task<IActionResult> NewTechStackRow()
        {
            var viewHtml = this.RenderViewAsync("~/Areas/Admin/Views/Partial/_TechStack.cshtml", string.Empty, true).GetAwaiter().GetResult();

            return Json(JsonWrapper.buildSuccessJson(viewHtml));
        }
    }
}
