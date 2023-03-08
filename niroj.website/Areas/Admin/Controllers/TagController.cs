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
    [Route("admin/tag")]
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IBaseRepository<Tag> _tagRepo;
        private readonly ILog _logService;

        public TagController(ITagService tagService, IBaseRepository<Tag> tagRepo, ILog logService)
        {
            _tagService = tagService;
            _tagRepo = tagRepo;
            _logService = logService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllUndeleted();
            return View(tags);
        }

        [HttpGet]
        [Route("save-update-view")]
        public async Task<IActionResult> SaveOrUpdateView([FromQuery] int tag_id)
        {
            var tag = await _tagRepo.FindAsync(a => a.Id == tag_id);
            var dto = new TagDto();
            if (tag != null)
            {
                dto = new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                };
            }

            var viewHtml = this.RenderViewAsync("~/Areas/Admin/Views/Partial/_TagAddUpdateView.cshtml", dto, true).GetAwaiter().GetResult();

            return Json(JsonWrapper.buildSuccessJson(viewHtml));
        }

        [HttpGet]
        [Route("check-duplicate-name")]
        public async Task<IActionResult> CheckDuplicateName(TagDto model)
        {
            bool isDuplicate = await _tagService.IsNameDuplicate(model);

            return Json(JsonWrapper.buildSuccessJson(isDuplicate));
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveOrUpdate(TagDto model)
        {
            string errorMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    await _tagService.Save(model);

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
                errorMessage = "Failed to save/update Tag";
                _logService.Error($"Failed to save/update tag, {ex}");
            }
            return Json(JsonWrapper.buildErrorJson(errorMessage));
        }


        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(long tag_id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _tagService.Delete(tag_id);

                    AlertHelper.setMessage(this, "Tag deleted successfully");
                    return RedirectToAction("index");
                }
                AlertHelper.setMessage(this, "Failed to delete tag.", MessageType.error);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to delete tag.", MessageType.error);
                _logService.Error($"Failed to delete tag , {ex}");
            }
            return RedirectToAction("index");
        }

    }
}
