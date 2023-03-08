using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Controllers;
using niroj.website.Extensions;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Implementations;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SG.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/blog-category")]
    public class BlogCategoryController : BaseController
    {
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IBlogCategoryRepository _blogCategoryRepo;
        private readonly ILog _logService;

        public BlogCategoryController(IBlogCategoryService blogCategoryService, IBlogCategoryRepository blogCategoryRepo,ILog logService)
        {
            _blogCategoryService = blogCategoryService;
            _blogCategoryRepo = blogCategoryRepo;
            _logService = logService;
        }

        [Route("")]
        [Route("index")]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var datas = _blogCategoryService.GetAll();
                return View(datas);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to get blog categories.", MessageType.error);
                _logService.Error($"Failed to get blog category list  ,{ex}");
            }
            return View(new List<BlogCategoryDto>());
        }

        [HttpGet]
        [Route("save-update-view")]
        public IActionResult SaveOrUpdateView([FromQuery] long category_id)
        {
            var category = new BlogCategoryDto();

            if (category_id > 0)
            {
                category = _blogCategoryService.GetById(category_id) ?? throw new CustomException("Blog Category not found.");

            }

            var viewHtml = this.RenderViewAsync("~/Areas/Admin/Views/Partial/_BlogCategoryAddUpdateView.cshtml", category, true).GetAwaiter().GetResult();

            return Json(JsonWrapper.buildSuccessJson(viewHtml));
        }

        [HttpGet]
        [Route("check-duplicate-name")]
        public IActionResult CheckDuplicateName(BlogCategoryDto model)
        {
            bool isDuplicate = CheckDuplicateCategoryName(model);

            return Json(JsonWrapper.buildSuccessJson(isDuplicate));
        }

        [HttpPost]
        [Route("save")]
        public IActionResult SaveOrUpdate(BlogCategoryDto model)
        {
            string errorMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    model.PerformedBy = getLoggedInUserId();
                    if (model.Id == 0)
                    {
                        _blogCategoryService.Save(model);
                    }
                    else
                    {
                        _blogCategoryService.Update(model);
                    }
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
                errorMessage = "Failed to save/update Blog Category";
                _logService.Error($"Failed to save/update blog category ,{ex}");
            }
            return Json(JsonWrapper.buildErrorJson(errorMessage));
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete(long category_id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _blogCategoryService.Delete(category_id);

                    AlertHelper.setMessage(this, "Blog Category deleted successfully");
                    return RedirectToAction(nameof(Index));
                }
                AlertHelper.setMessage(this, "Failed to delete blog category.", MessageType.error);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to delete blog category.", MessageType.error);
                _logService.Error($"Failed to delete blog category ,{ex}");
            }
            return RedirectToAction("index");
        }

        private bool CheckDuplicateCategoryName(BlogCategoryDto model)
        {
            var categoryWithSameName = _blogCategoryRepo.GetQueryable().Where(a => a.Title.ToLower().Trim().Equals(model.Title.ToLower().Trim())).SingleOrDefault();

            if (categoryWithSameName == null)
            {
                return false;
            }

            return categoryWithSameName.Id != model.Id;
        }
    }
}
