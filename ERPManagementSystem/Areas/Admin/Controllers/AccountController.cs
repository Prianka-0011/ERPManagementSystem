using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Models;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
           ILogger<AccountController> logger, IEmailSender emailSender, RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context, UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var user = _context.Users.FirstOrDefault(c => c.UserName == login.UserName || c.Email == login.UserName);
                    var role = _context.UserRoles.FirstOrDefault(c => c.UserId == user.Id);
                    var roleType = _context.Roles.FirstOrDefault(c => c.Id == role.RoleId);
                    if (roleType.Name == "Admin")
                    {
                        //   return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Index", _context.PaymentBankingType.ToList()) });
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                }
            }
            return View();
        }
      
    }
}
