using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Services.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    public class GithubController : Controller
    {
        private const int _gitRepoCountInDashboard = 8;
        private readonly IGithubService _githubService;
        public GithubController(IGithubService githubService)
        {
            _githubService = githubService;
        }

        public async Task<IActionResult> Public()
        {
            var publicRepos = await _githubService.GetAllPublicRepos();
            return PartialView("~/Views/Github/_RepoList.cshtml", publicRepos.OrderByDescending(a => a.StargazersCount).Take(_gitRepoCountInDashboard).ToList());
        }
    }
}
