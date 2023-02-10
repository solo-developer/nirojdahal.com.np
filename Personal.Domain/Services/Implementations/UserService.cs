using Microsoft.AspNetCore.Identity;
using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace Personal.Domain.Services.Implementations
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IApplicationUserRepository _applicationUserRepo;
        private readonly IUserAssembler _userAssembler;
        private readonly IFileHelper _fileHelper;
        private const string IMAGE_FOLDER = "uploads/user-img";
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepo, IApplicationUserRepository applicationUserRepo, IUserAssembler userAssembler, IFileHelper fileHelper, IPasswordHasher<ApplicationUser> passwordHash, UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _applicationUserRepo = applicationUserRepo;
            _userAssembler = userAssembler;
            _fileHelper = fileHelper;
            _passwordHasher = passwordHash;
            _userManager = userManager;
        }

        public void Edit(UserSaveDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                if (IsUsernameDuplicate(dto))
                {
                    throw new DuplicateItemException("User with specified username already exists.");
                }

                var user = _userRepo.GetById(dto.UserId) ?? throw new ItemNotFoundException("User doesnot exist.");

                string userImg = user.ImageName;
                var applicationUser = user.AuthenticationDetail;
                applicationUser.UserName = dto.UserName;
                applicationUser.Email = dto.Email;
                _applicationUserRepo.Update(applicationUser);

                _userAssembler.Copy(dto, user);

                _userRepo.Update(user);

                if (!string.IsNullOrWhiteSpace(dto.ImageName))
                {
                    _fileHelper.MoveImageFromTempPathToDestination(dto.ImageName, IMAGE_FOLDER);
                };
                if (!string.IsNullOrWhiteSpace(userImg))
                {

                    _fileHelper.DeleteFile(IMAGE_FOLDER, userImg);
                }

                tx.Complete();
            }
        }

        public List<UserDto> GetAll()
        {
            var users = _userRepo.GetAll().ToList();

            var allAuthenticationIds = users.Select(a => a.IdentityUserId).ToList();

            var authenticationDetails = _applicationUserRepo.GetQueryable().Where(a => allAuthenticationIds.Contains(a.Id)).ToList();

            List<UserDto> response = new List<UserDto>();
            foreach (var userDetail in users)
            {
                var authenticationDetail = authenticationDetails.Single(a => a.Id == userDetail.IdentityUserId);

                var userDto = new UserDto();
                Copy(userDetail, authenticationDetail, userDto);
                response.Add(userDto);
            }

            return response;
        }

        public UserDto GetById(long id)
        {
            var userDetail = _userRepo.GetQueryable().Where(a => a.Id == id).SingleOrDefault();
            if (userDetail == null)
            {
                return null;
            }

            var authenticationDetail = userDetail.AuthenticationDetail;

            var userDto = new UserDto();
            Copy(userDetail, authenticationDetail, userDto);

            return userDto;
        }
        public UserDto GetByAspUserId(string id)
        {
            var userDetail = _userRepo.GetQueryable().Where(a => a.AuthenticationDetail.Id == id).SingleOrDefault();
            if (userDetail == null)
            {
                return null;
            }

            var authenticationDetail = userDetail.AuthenticationDetail;

            var userDto = new UserDto();
            Copy(userDetail, authenticationDetail, userDto);

            return userDto;
        }

        private void Copy(UserDetail userDetail, ApplicationUser authenticationDetail, UserDto userDto)
        {
            userDto.UserId = userDetail.Id;
            userDto.UserName = authenticationDetail.UserName;
            userDto.ImageName = userDetail.ImageName;
            userDto.MobileNo = userDetail.MobileNo;
            userDto.Email = authenticationDetail.Email;
            userDto.FullName = userDetail.FullName;
            userDto.Address = userDetail.Address;
        }

        public void Save(UserSaveDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                if (IsUsernameDuplicate(dto))
                {
                    throw new DuplicateItemException("User with specified username already exists.");
                }

                ApplicationUser user = new ApplicationUser
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    NormalizedUserName = dto.UserName.ToUpper(),
                    NormalizedEmail = dto.Email?.ToUpper(),

                };

                _applicationUserRepo.Insert(user);
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
                _applicationUserRepo.Update(user);

                var entity = new UserDetail();
                _userAssembler.Copy(dto, entity);
                entity.AuthenticationDetail = user;
                entity.IdentityUserId = user.Id;

                _userRepo.Insert(entity);

                if (!string.IsNullOrWhiteSpace(dto.ImageName))
                {
                    _fileHelper.MoveImageFromTempPathToDestination(dto.ImageName, IMAGE_FOLDER);
                };

                tx.Complete();
            }
        }

        private bool IsUsernameDuplicate(UserSaveDto dto)
        {

            var userWithSameUsername = _applicationUserRepo.GetQueryable().Where(a => a.UserName == dto.UserName).SingleOrDefault();

            if (userWithSameUsername == null)
            {
                return false;
            }

            string aspUserId = userWithSameUsername.Id;

            var userDetailWithSpecifiedUsername = _userRepo.GetById(dto.UserId);

            if (userDetailWithSpecifiedUsername == null)
            {
                return false;
            }

            return dto.UserId != userDetailWithSpecifiedUsername.Id;
        }

        public async Task ChangePassword(ChangePasswordDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var user = _applicationUserRepo.Find(a => a.Id == dto.AspUserId) ?? throw new ItemNotFoundException("User not found.");
                var isCurrentPasswordMatched = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);

                if(!isCurrentPasswordMatched)
                {
                    throw new InvalidValueException("Current password didn't match.");
                }
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

                _applicationUserRepo.Update(user);
                tx.Complete();
            }


        }
    }
}
