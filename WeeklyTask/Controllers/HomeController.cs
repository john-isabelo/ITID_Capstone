using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics;
using WeeklyTask.Areas.Identity.Data;
using WeeklyTask.Models;
using WeeklyTask.Models.Helpers;

namespace WeeklyTask.Controllers
{

    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IOptions<StripeSettings> _stripeSettings;

        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<StripeSettings> stripeSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _stripeSettings = stripeSettings;
        }

        public async Task<IActionResult> Index()
        {
            var adminUser = await _userManager.FindByEmailAsync("admin@test.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Email = "admin@test.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin@test.com",
                    NormalizedEmail = "admin@test.com"
                };
                var result = await _userManager.CreateAsync(adminUser, "Password@123");
                if (result.Succeeded)
                {
                    // Admin user created successfully
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("admin"));
                    if (roleResult.Succeeded)
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, "admin");
                        if (addToRoleResult.Succeeded)
                        {
                            // User added to admin role successfully
                        }
                    }
                }

            }

            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                if (roleResult.Succeeded)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
                    if (addToRoleResult.Succeeded)
                    {
                        // User added to admin role successfully
                    }
                }
            }


            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}