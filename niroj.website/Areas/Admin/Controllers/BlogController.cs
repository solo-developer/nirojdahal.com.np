using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Controllers;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
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

        public BlogController(IBlogService blogService, IBlogCategoryService blogCategoryService, IFileHelper fileHelper, ITagService tagService)
        {
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _fileHelper = fileHelper;
            _tagService = tagService;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var datas = await _blogService.GetAll(0);
                return View(datas.Data as List<BlogDto>);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception)
            {
                AlertHelper.setMessage(this, "Failed to get blogs.", MessageType.error);
            }
            return View(new List<BlogDto>());
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
            catch (System.Exception)
            {
                AlertHelper.setMessage(this, "Failed to get view.", MessageType.error);
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
                    if (dto.Banner != null)
                    {
                        string tempPath = Path.GetTempPath();
                        dto.BannerImage = await _fileHelper.saveImageAndGetFileName(dto.Banner, tempPath, dto.Title);
                    }
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
                        dto.BannerImage = await _fileHelper.saveImageAndGetFileName(dto.Banner, tempPath, dto.Title);
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
            }
            return RedirectToAction(nameof(Index));
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
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
