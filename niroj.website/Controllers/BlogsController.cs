using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Services.Interface;
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
        public async Task<IActionResult> Index(int? skip = null, int? take = null)
        {
            var blogs = await _blogService.GetAll(skip.HasValue ? skip.Value : 0, take);
            return View(blogs);
        }

        [Route("{slug}")]
        public async Task<IActionResult> GetBlog(string slug)
        {
            var blog = await _blogService.GetBySlug(slug);
            return View("BlogDetail",blog);
        }

        [Route("dashboard-section")]
        public async Task<IActionResult> GetBlogsForDashboard()
        {
            var blogs = await _blogService.GetAll(0, 3);
            return PartialView("~/Views/Blogs/_blogDashboardList.cshtml", blogs);
        }
    }
}
