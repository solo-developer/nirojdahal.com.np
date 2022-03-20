using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Areas.UserManagement.Models;
using niroj.website.Controllers;
using niroj.website.Helpers;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Areas.UserManagement.Controllers
{
    [Authorize]
    [Area("UserManagement")]
    [Route("user-management/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleRepository _roleRepo;
        private readonly IFileHelper _fileHelper;

        public UserController(IUserService userService, IRoleRepository roleRepo, IFileHelper fileHelper)
        {
            _userService = userService;
            _roleRepo = roleRepo;
            _fileHelper = fileHelper;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        [Authorize(Policy = "UserManagement")]
        public IActionResult Index()
        {
            var users = _userService.GetAll();
            return View(users);
        }

        [Route("new")]
        [HttpGet]
        [Authorize(Policy = "UserManagement")]
        public IActionResult Save()
        {
            ViewBag.roles = _roleRepo.GetQueryable().Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return View(new UserSaveViewModel());
        }

        [Route("save")]
        [HttpPost]
        [Authorize(Policy = "UserManagement")]
        public async Task<IActionResult> Save(UserSaveViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserSaveDto dto = new UserSaveDto();
                    await Copy(model, dto);
                    _userService.Save(dto);

                    return RedirectToAction("index");
                }
                return View(model);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save user", MessageType.error);
            }
            finally
            {
                ViewBag.roles = _roleRepo.GetQueryable().Select(a => new
                {
                    a.Id,
                    a.Name
                }).ToList();
            }
            return RedirectToAction("index");
        }

        [Route("edit")]
        [HttpGet]
        //   [Authorize(Policy = "UserManagement")]
        public IActionResult Edit(long user_id)
        {
            try
            {
                var user = _userService.GetById(user_id) ?? throw new ItemNotFoundException("User doesnot exist.");
                ViewBag.roles = _roleRepo.GetQueryable().Select(a => new
                {
                    a.Id,
                    a.Name
                }).ToList();

                var vm = new UserSaveViewModel()
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    UserName = user.UserName,
                    ImageName = user.ImageName,
                    FullName = user.FullName,
                };

                return View("Save", vm);

            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to load view", MessageType.error);
            }
            return RedirectToAction("index");
        }

        [Route("edit")]
        [HttpPost]
        //   [Authorize(Policy = "UserManagement")]
        public async Task<IActionResult> Edit(UserSaveViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserSaveDto dto = new UserSaveDto();
                    await Copy(model, dto);
                    _userService.Edit(dto);

                    return RedirectToAction("index");
                }
                return View(model);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to save user", MessageType.error);
            }
            finally
            {
                ViewBag.roles = _roleRepo.GetQueryable().Select(a => new
                {
                    a.Id,
                    a.Name
                }).ToList();
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            try
            {
                var userId = getLoggedInUserId();
                var userDto = _userService.GetByAspUserId(userId);
                return View(userDto);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to get user profile.", MessageType.error);
            }
            return Redirect("/");
        }

        private async Task Copy(UserSaveViewModel model, UserSaveDto dto)
        {
            dto.UserId = model.UserId;
            dto.Password = model.Password;
            dto.UserId = model.UserId;
            dto.UserName = model.UserName;
            dto.FullName = model.FullName;
            dto.MobileNo = model.MobileNo;
            dto.Email = model.Email;

            if (model.Image != null)
            {
                string tempPath = Path.GetTempPath();
                string fileName = await _fileHelper.saveImageAndGetFileName(model.Image, tempPath, model.FullName.Replace(" ", "-"));

                dto.ImageName = fileName;
            }
        }
    }
}
