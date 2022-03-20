using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Areas.UserManagement.Models;
using niroj.website.Extensions;
using niroj.website.Helpers;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace niroj.website.Areas.UserManagement.Controllers
{
    [Authorize]
    [Area("UserManagement")]
    [Route("user-management/role")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleRepository _roleRepo;
        public RoleController(RoleManager<IdentityRole> roleManager, IRoleRepository roleRepo)
        {
            _roleManager = roleManager;
            _roleRepo = roleRepo;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
      //  [Authorize(Policy ="RoleManagement")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();

            var rolesVM = roles.Select(a => new RoleIndexViewModel
            {
                Id = a.Id,
                Name = a.Name,
                NormalizedName = a.NormalizedName
            }).ToList();

            return View(rolesVM);
        }

        [HttpGet]
        [Route("save-update-view")]
       // [Authorize(Policy = "RoleManagement")]
        public async Task<IActionResult> SaveOrUpdateView([FromQuery] string role_id)
        {
            var role = new RoleIndexViewModel();

            if (!string.IsNullOrWhiteSpace(role_id))
            {
                var roleEntity = await _roleManager.FindByIdAsync(role_id) ?? throw new ItemNotFoundException("Role doesnot exist.");
                role = new RoleIndexViewModel()
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name,
                    NormalizedName = roleEntity.NormalizedName
                };
            }

            var viewHtml = this.RenderViewAsync("~/Areas/UserManagement/Views/Partial/_RoleAddUpdateView.cshtml", role, true).GetAwaiter().GetResult();

            return Json(JsonWrapper.buildSuccessJson(viewHtml));
        }

        [HttpGet]
        [Route("check-duplicate-name")]
       // [Authorize(Policy = "RoleManagement")]
        public IActionResult CheckDuplicateName(RoleSaveViewModel model)
        {
            bool isDuplicate = CheckDuplicateRole(model);

            return Json(JsonWrapper.buildSuccessJson(isDuplicate));
        }

        [HttpPost]
        [Route("save")]
       // [Authorize(Policy = "RoleManagement")]
        public async Task<IActionResult> SaveOrUpdate(RoleSaveViewModel model)
        {
            string errorMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = null;
                    if (string.IsNullOrEmpty(model.Id))
                    {
                        var role = new IdentityRole()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = model.Name,
                            NormalizedName = model.Name.ToUpper()
                        };

                        result = await _roleManager.CreateAsync(role);
                    }
                    else
                    {
                        var role = await _roleManager.FindByIdAsync(model.Id) ?? throw new CustomException("Role doesnot exist.");
                        role.Name = model.Name;
                        role.NormalizedName = model.Name.ToUpper();

                        result = await _roleManager.UpdateAsync(role);
                    }

                    if (result.Succeeded)
                    {
                        return Json(JsonWrapper.buildSuccessJson(new { success = true }));
                    }
                }
                errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            }
            catch (CustomException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Failed to save/update Role";
            }
            return Json(JsonWrapper.buildErrorJson(errorMessage));
        }

        private bool CheckDuplicateRole(RoleSaveViewModel model)
        {
            var roleWithSameName = _roleRepo.GetQueryable().Where(a => a.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())).SingleOrDefault();

            if (roleWithSameName == null)
            {
                return false;
            }

            return roleWithSameName.Id != model.Id;
        }

        [HttpGet]
        [Route("delete")]
       // [Authorize(Policy = "RoleManagement")]
        public async Task<IActionResult> Delete(string role_id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = await _roleManager.FindByIdAsync(role_id) ?? throw new CustomException("Role doesnot exist.");

                    if (_roleRepo.AreUsersPresentInRole(role.Name))
                    {
                        throw new CustomException("Role cannot be deleted when it is assigned to active user.");
                    }
                    var result = await _roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        AlertHelper.setMessage(this, "Role deleted successfully");
                        return RedirectToAction("index");
                    }
                }
                AlertHelper.setMessage(this, "Failed to delete role.", MessageType.error);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to delete role.", MessageType.error);
            }
            return RedirectToAction("index");
        }
    }
}
