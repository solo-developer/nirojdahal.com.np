using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
