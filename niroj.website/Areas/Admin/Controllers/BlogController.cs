using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Areas.UserManagement.Models;
using niroj.website.Controllers;
using niroj.website.Extensions;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/blog")]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IFileHelper _fileHelper;
        private readonly ITagService _tagService;
        private readonly ILog _logService;

        public BlogController(IBlogService blogService, IBlogCategoryService blogCategoryService, IFileHelper fileHelper, ITagService tagService, ILog logService)
        {
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _fileHelper = fileHelper;
            _tagService = tagService;
            _logService = logService;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var filterDto = new BlogFilterDto { Take = null };
                var datas = await _blogService.GetAll(filterDto);
                return View(datas.Data as List<BlogDto>);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to get blogs.", MessageType.error);
                _logService.Error($"Failed to get blogs ,{ex}");
            }
            return View(new List<BlogDto>());
        }

        [HttpGet]
        [Route("rating-view")]
        public async Task<IActionResult> SaveOrUpdateView([FromQuery] int blogId)
        {
            var rating = new BlogRatingDto
            {
                BlogId = blogId
            };
            var viewHtml = this.RenderViewAsync("~/Areas/admin/Views/Blog/Partial/_BlogRatingView.cshtml", rating, true).GetAwaiter().GetResult();

            return Json(JsonWrapper.buildSuccessJson(viewHtml));
        }

        [Route("new")]
        [HttpGet]
        // [Authorize(Policy = "AddBlog")]
        public async Task<IActionResult> New()
        {
            try
            {
                var categories = _blogCategoryService.GetAll();
                ViewBag.categories = categories;
                var tags = await _tagService.GetAllUndeleted();
                ViewBag.TagOptions = tags;
                return View(new BlogDto());
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to get view.", MessageType.error);
                _logService.Error($"Failed to get new blog view ,{ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("save")]
        [HttpPost]
        // [Authorize(Policy = "AddBlog")]
        public async Task<IActionResult> Save(BlogDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (dto.Banner == null) throw new NonNullValueException("Banner image is required");

                    string tempPath = Path.GetTempPath();
                    dto.BannerImage = await _fileHelper.SaveImageAndGetFileName(dto.Banner, tempPath, dto.Title);

                    dto.PerformedBy = getLoggedInUserId();
                    _blogService.Save(dto);
                    AlertHelper.setMessage(this, "Blog Saved Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save blog.", MessageType.error);
                _logService.Error($"Failed to save blog ,{ex}");
            }
            finally
            {
                var categories = _blogCategoryService.GetAll();
                ViewBag.categories = categories;
                var tags = await _tagService.GetAllUndeleted();
                ViewBag.TagOptions = tags;
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var blog = _blogService.GetById(id);
                var categories = _blogCategoryService.GetAll();
                ViewBag.categories = categories;
                var tags = await _tagService.GetAllUndeleted();
                ViewBag.TagOptions = tags;
                return View("New", blog);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to get edit blog view ,{ex}");
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(BlogDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dto.PerformedBy = getLoggedInUserId();
                    if (dto.Banner != null)
                    {
                        string tempPath = Path.GetTempPath();
                        dto.BannerImage = await _fileHelper.SaveImageAndGetFileName(dto.Banner, tempPath, dto.Title);
                    }
                    _blogService.Update(dto);
                    AlertHelper.setMessage(this, "Blog updated Successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to update blog.", MessageType.error);
                _logService.Error($"Failed to update blog ,{ex}");
            }
            finally
            {
                var categories = _blogCategoryService.GetAll();
                ViewBag.categories = categories;
                var tags = await _tagService.GetAllUndeleted();
                ViewBag.TagOptions = tags;
            }
            return View("New", dto);
        }

        [HttpGet]
        [Route("publish/{id}")]
        //  [Authorize(Policy = "UpdateBlog")]
        public IActionResult Publish(long id)
        {
            try
            {
                _blogService.Publish(id, getLoggedInUserId());
                AlertHelper.setMessage(this, "Blog published successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to publish blog ,{ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("save-rating")]
        //  [Authorize(Policy = "UpdateBlog")]
        public async Task<IActionResult> SaveRating([FromForm] BlogRatingDto dto)
        {
            try
            {
                await _blogService.SaveRating(dto);
                return Json(JsonWrapper.buildSuccessJson(new { success = true }));
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to rate blog ,{ex}");
                return Json(JsonWrapper.buildErrorJson("Failed to rate blog"));
            }
        }

        [HttpGet]
        [Route("unpublish/{id}")]
        // [Authorize(Policy = "UpdateBlog")]
        public IActionResult Unpublish(long id)
        {
            try
            {
                _blogService.Unpublish(id, getLoggedInUserId());
                AlertHelper.setMessage(this, "Blog unpublished successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to unpublish blog ,{ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("delete/{blog_id}")]
        //  [Authorize(Policy = "DeleteBlog")]
        public IActionResult Delete(long blog_id)
        {
            try
            {
                _blogService.Delete(blog_id, getLoggedInUserId());
                AlertHelper.setMessage(this, "Blog deleted successfully.", MessageType.success);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
                _logService.Error($"Failed to delete blog ,{ex}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
