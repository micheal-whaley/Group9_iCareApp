using Microsoft.AspNetCore.Mvc;

namespace Group9_iCareApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult Create()
        {
            return Redirect("~/Identity/Account/Register");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
