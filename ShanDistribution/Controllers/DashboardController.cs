


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShanDistribution.Controllers
{
    // 1. Controller-Level: Only authenticated users with Admin OR Manager roles can access any action here
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        // Inherits the controller-level rule: accessible by Admin and Manager
        public IActionResult Index()
        {
            return View();
        }

        // 2. Action-Level Override (Narrowing): Only Admin can access this action
        [Authorize(Roles = "Admin")]
        public IActionResult SystemSettings()
        {
            return View();
        }

        // 3. Action-Level Expansion: Accessible by Admin, Manager, OR Rep
        [Authorize(Roles = "Admin,Manager,Rep")]
        public IActionResult Reports()
        {
            return View();
        }

        // 4. Require Multiple Roles Simultaneously (Admin AND Manager)
        // Note: Chaining separate attributes acts as a logical AND
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Manager")]
        public IActionResult SuperOperations()
        {
            return View();
        }

        // 5. Allow Public Access inside an Authorized Controller
        [AllowAnonymous]
        public IActionResult PublicNotice()
        {
            return View();
        }
    }
}