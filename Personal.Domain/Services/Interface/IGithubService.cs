using Personal.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IGithubService
    {
        Task<List<GithubRepoDto>> GetAllPublicRepos();
    }
}
