using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace niroj.website.Controllers
{
    public class BaseController : Controller
    {
        protected void clearUserCookies()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
        }
        protected string getLoggedInUserId()
        {
            string userId = Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
