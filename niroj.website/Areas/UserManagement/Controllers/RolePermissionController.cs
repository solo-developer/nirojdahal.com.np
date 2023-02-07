using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Areas.UserManagement.Models;
using niroj.website.Helpers;
using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace niroj.website.Areas.UserManagement.Controllers
{
    [Authorize]
    [Area("UserManagement")]
    [Route("user-management/role-permission")]
    public class RolePermissionController : Controller
    {
        private readonly IRolePermissionService _service;
        private readonly IRoleRepository _roleRepo;
        private readonly IPermissionRepository _permissionRepo;
        private readonly IBaseRepository<Module> _moduleRepo;
        private readonly IRolePermissionRepository _rolePermissionRepo;

        public RolePermissionController(IRolePermissionService service, IRoleRepository roleRepo, IPermissionRepository permissionRepo, IBaseRepository<Module> moduleRepo, IRolePermissionRepository rolePermissionRepo)
        {
            _service = service;
            _roleRepo = roleRepo;
            _permissionRepo = permissionRepo;
            _moduleRepo = moduleRepo;
            _rolePermissionRepo = rolePermissionRepo;
        }
        [Route("")]
        [HttpGet]
        [Route("index")]
        //[Authorize(Policy = "RolePermissionManagement")]
        public IActionResult Index()
        {
            try
            {
                var roles = _roleRepo.GetQueryable().Select(a => new RoleViewModel
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList();

                ViewBag.roles = roles;

                var modules = _moduleRepo.GetAll();
                var permissions = _permissionRepo.GetAll();

                var permissionTreeView = BuildTreeView(modules, permissions);

                return View(permissionTreeView);

            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to load role permissions.", MessageType.error);
            }
            return Redirect("/");
        }

        [Route("permissions-of-role")]
        [HttpGet]
       // [Authorize(Policy = "RolePermissionManagement")]
        public IActionResult GetPermissionsOfRole(string role_id)
        {
            try
            {
                var permissions = _rolePermissionRepo.GetPermissionsOfRole(role_id);
                return Json(JsonWrapper.buildSuccessJson(permissions));
            }
            catch (Exception ex)
            {
                return Json(JsonWrapper.buildErrorJson("Failed to get permissions of role."));
            }
        }

        [Route("save")]
        [HttpPost]
      //  [Authorize(Policy = "RolePermissionManagement")]
        public IActionResult Save([FromBody]RolePermissionSaveViewModel vm)
        {
            try
            {
               _service.SaveOrUpdate(vm.RoleId, vm.Permissions);
                return Json(JsonWrapper.buildSuccessJson("Role Permission saved successfully"));
            }
            catch (Exception ex)
            {
                return Json(JsonWrapper.buildErrorJson("Failed to get permissions of role."));
            }
        }

        private List<PermissionTreeViewModel> BuildTreeView(List<Module> modules, List<Permission> permissions)
        {
            List<PermissionTreeViewModel> response = new List<PermissionTreeViewModel>();
            foreach (var module in modules)
            {
                var moduleTreeView = new PermissionTreeViewModel();
                moduleTreeView.DisplayName = module.Name;
                moduleTreeView.Name = module.Name;
                moduleTreeView.IsHeader = true;
                var permissionsInsideModules = permissions.Where(a => a.ModuleId == module.Id).ToList();

                var rootPermissions = permissionsInsideModules.Where(a => a.ParentId == null).ToList();
                foreach (var permission in rootPermissions)
                {
                    moduleTreeView.Nodes.Add(GetNode(permission, permissions));
                }
                response.Add(moduleTreeView);
            }
            return response;
        }

        private PermissionTreeViewModel GetNode(Permission permission, List<Permission> permissions)
        {
            PermissionTreeViewModel response = new PermissionTreeViewModel();
            response.Name = permission.Name;
            response.DisplayName = permission.DisplayName;
            var children = permissions.Where(a => a.ParentId == permission.Id).ToList();
            response.IsHeader = children.Any();
            foreach (var child in children)
            {
                response.Nodes.Add(GetNode(child, permissions));
            }
            return response;
        }
    }
}
