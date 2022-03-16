using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;

namespace Personal.Domain.Services.Implementations
{
    public class LastReadService : ILastReadService
    {
        private readonly IBaseRepository<LastRead> _repo;
        private readonly IApplicationUserRepository _userRepo;
        public LastReadService(IBaseRepository<LastRead> repo,IApplicationUserRepository userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        public void Record(ReadableTableKeys key, string userId)
        {
            LastRead entity = new LastRead()
            {
                Key=key,
                ReadBy=userId,
                ReadDate=DateTime.Now,
                User=_userRepo.GetQueryable().SingleOrDefault(a=>a.Id==userId) ?? throw new ItemNotFoundException("User not found.")
            };
            _repo.Insert(entity);
        }
    }
}
