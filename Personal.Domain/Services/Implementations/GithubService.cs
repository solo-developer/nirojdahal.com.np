using Octokit;
using Personal.Domain.Dto;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class GithubService : IGithubService
    {
        private readonly ICryptoGraphyService _cryptoGraphyService;
        public GithubService(ICryptoGraphyService cryptoGraphyService)
        {
            _cryptoGraphyService = cryptoGraphyService;
        }
        public async Task<List<GithubRepoDto>> GetAllPublicRepos()
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var encryptedToken = "/bdhYPh1Xgm9IJBBOu7D21sNxN8UckU0fWjJXcE9sS3Uo4C0D073bzipVsMfPZ7X";
            var decryptedToken= _cryptoGraphyService.DecryptString(key,encryptedToken);
            var tokenAuth = new Credentials(decryptedToken);

            var client = new GitHubClient(new ProductHeaderValue("OrgnizationName"));
            client.Credentials = tokenAuth;
            //Search of all the repositories for given user account
            var repos = await client.Repository.GetAllForCurrent();

            var publicRepos = repos.Where(a => !a.Private).ToList();

            List<GithubRepoDto> response = new List<GithubRepoDto>();
            foreach (var repo in publicRepos)
            {
                response.Add(new GithubRepoDto
                {
                    Url = repo.Url,
                    HtmlUrl = repo.HtmlUrl,
                    CloneUrl = repo.CloneUrl,
                    GitUrl = repo.GitUrl,
                    SshUrl = repo.SshUrl,
                    SvnUrl = repo.SvnUrl,
                    MirrorUrl = repo.MirrorUrl,
                    Id = repo.Id,
                    NodeId = repo.NodeId,
                    Name = repo.Name,
                    FullName = repo.FullName,
                    IsTemplate = repo.IsTemplate,
                    Description = repo.Description,
                    Homepage = repo.Homepage,
                    Language = repo.Language,
                    Private = repo.Private,
                    StargazersCount = repo.StargazersCount,
                    DefaultBranch = repo.DefaultBranch,
                });
            }

            return response;
        }
    }
}
