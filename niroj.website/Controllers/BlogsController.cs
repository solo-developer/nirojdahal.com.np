using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using niroj.website.ViewModels;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("blogs")]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryRepository _blogCategoryRepo;

        public BlogsController(IBlogService blogService, IBlogCategoryRepository blogCategoryRepo)
        {
            _blogService = blogService;
            _blogCategoryRepo = blogCategoryRepo;
        }


        [Route("index")]
        [Route("")]
        public async Task<IActionResult> Index(BlogFilterDto dto)
        {
            dto.OnlyPublished = true;
            dto.Take = 6;
            var blogs = await _blogService.GetAll(dto);
            return View(blogs);
        }

        [Route("{slug}")]
        public async Task<IActionResult> GetBlog(string slug)
        {
            var blog = await _blogService.GetBySlug(slug);

            var categories = _blogCategoryRepo.GetAllIncluding(a => a.Blogs).Select(a => new BlogCategoryCountViewModel
            {
                Id = a.Id,
                BlogsCount = a.Blogs.Where(b => b.IsPublished).Count(),
                Name = a.Title
            }).ToList();
            ViewBag.categories = categories;
            return View("BlogDetail", blog);
        }

        [Route("dashboard-section")]
        public async Task<IActionResult> GetBlogsForDashboard()
        {
            var fiterDto = new BlogFilterDto { Take = 3, PageNo = 1,OnlyPublished=true };
            var blogs = await _blogService.GetAll(fiterDto);
            return PartialView("~/Views/Blogs/_blogDashboardList.cshtml", blogs.Data as List<BlogDto>);
        }

        [HttpPost]
        [Route("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] NewsletterSubscriptionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                    return Json(JsonWrapper.buildErrorJson(allErrors.First()));
                }

                await _blogService.SubscribeNewsletter(dto);
                return Json(JsonWrapper.buildSuccessJson(true));

            }
            catch (CustomException ex)
            {
                return Json(JsonWrapper.buildErrorJson(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(JsonWrapper.buildErrorJson("Failed to subscribe newsletter."));
            }
        }
    }
}
