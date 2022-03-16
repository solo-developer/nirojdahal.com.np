using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Controllers;
using Personal.Domain.Services.Interface;

namespace niroj.website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Route("blog-category")]
    public class BlogCategoryController : BaseController
    {
        private readonly IBlogCategoryService _blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var allCategories = _blogCategoryService.GetAll();
            return View(allCategories);
        }
    }
}
