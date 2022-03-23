using Microsoft.AspNetCore.Mvc;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [Route("blogs")]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Route("index")]
        [Route("")]
        public async Task<IActionResult> Index(int? pageNo = 1, int? take = 6)
        {
            int skip = ((int)(pageNo.Value - 1)) * take.Value;
            var blogs = await _blogService.GetAll(skip, take);
            return View(blogs);
        }

        [Route("{slug}")]
        public async Task<IActionResult> GetBlog(string slug)
        {
            var blog = await _blogService.GetBySlug(slug);
            return View("BlogDetail", blog);
        }

        [Route("dashboard-section")]
        public async Task<IActionResult> GetBlogsForDashboard()
        {
            var blogs = await _blogService.GetAll(0, 3);
            return PartialView("~/Views/Blogs/_blogDashboardList.cshtml", blogs);
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
