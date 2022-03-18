using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Services.Interface;

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
        public IActionResult Index()
        {
            var blogs= _blogService.GetAll(0,3);
            return PartialView("~/Views/Blogs/_blogDashboardList.cshtml", blogs);
        }
    }
}
