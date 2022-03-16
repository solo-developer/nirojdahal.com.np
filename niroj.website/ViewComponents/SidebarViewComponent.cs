using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Services.Interface;
using System;
using System.Threading.Tasks;

namespace niroj.website.ViewComponents
{
    [ViewComponent(Name = "SidebarView")]
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public SidebarViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var loggedInUserId = Request.HttpContext.User.FindFirst("user_id")?.Value;
            //var loggedInUserDetail = _userService.GetById(Convert.ToInt64(loggedInUserId));

            return View();
        }
    }
}
