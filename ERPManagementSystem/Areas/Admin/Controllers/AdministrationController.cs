using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.srcString = searchString;
            ViewBag.roleName = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var roles = _roleManager.Roles;
            List<CreateRoleVm> createRoleVms = new List<CreateRoleVm>();
            CreateRoleVm roleVm;
            switch (sortOrder)
            {
                case "prod_desc":
                    roles = roles.OrderByDescending(n => n.Name);
                    break;
                default:
                    roles = roles.OrderBy(n => n.Name);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                roles = _roleManager.Roles.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
            }
            foreach (var item in roles)
            {
                roleVm = new CreateRoleVm();
                roleVm.RoleName = item.Name;
                roleVm.Id = item.Id;
                createRoleVms.Add(roleVm);
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }

            var resCount = createRoleVms.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = createRoleVms.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(data);
        }
        //AddOrEdit
        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> AddOrEdit(string id)
        {

            return View(new CreateRoleVm());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(string roleId, CreateRoleVm vm)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                CreateRoleVm roleVm;
                List<CreateRoleVm> createRoleVms = new List<CreateRoleVm>();
                int pg = 1;
                const int pageSize = 10;
                if (roleId == null)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = vm.RoleName
                    };
                    result = await _roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {

                        if (pg < 1)
                        {
                            pg = 1;
                        }
                        var roles = _roleManager.Roles;
                        foreach (var item in roles)
                        {
                            roleVm = new CreateRoleVm();
                            roleVm.RoleName = item.Name;
                            createRoleVms.Add(roleVm);
                        }
                        var resCount = createRoleVms.Count();
                        ViewBag.TotalRecord = resCount;

                        var pager = new Pager(resCount, pg, pageSize);
                        int resSkip = (pg - 1) * pageSize;
                        var data = createRoleVms.Take(pager.PageSize);
                        ViewBag.Pager = pager;
                        return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllRole", data) });

                    }
                }

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", vm) });

        }
        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {



            var role = _roleManager.Roles.Where(c => c.Id == id).FirstOrDefault();
            if (role == null)
            {
                return NotFound();
            }
            CreateRoleVm roleVm = new CreateRoleVm();
            roleVm.Id = role.Id;
            roleVm.RoleName = role.Name;
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    roleVm.Users.Add(user.UserName);
                }
            }
            return View(roleVm);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateRoleVm vm)
        {
            if (ModelState.IsValid)
            {

                CreateRoleVm roleVm;
                List<CreateRoleVm> createRoleVms = new List<CreateRoleVm>();
                int pg = 1;
                const int pageSize = 10;

                var result = await _roleManager.FindByIdAsync(vm.Id);
                if (result == null)
                {
                    return NotFound();

                }
                else
                {
                    result.Name=vm.RoleName;
                  await  _roleManager.UpdateAsync(result);

                }
                if (pg < 1)
                {
                    pg = 1;
                }
                var roles = _roleManager.Roles;
                foreach (var item in roles)
                {
                    roleVm = new CreateRoleVm();
                    roleVm.RoleName = item.Name;
                    createRoleVms.Add(roleVm);
                }
                var resCount = createRoleVms.Count();
                ViewBag.TotalRecord = resCount;

                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = createRoleVms.Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllRole", data) });




            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", vm) });

        }
    }
}
