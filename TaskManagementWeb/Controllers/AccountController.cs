using Microsoft.AspNetCore.Mvc;

namespace TaskManagementWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Clear session/token
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
