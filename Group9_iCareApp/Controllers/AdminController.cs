using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Group9_iCareApp.Models;

namespace Group9_iCareApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly iCAREDBContext context;

        public AdminController(iCAREDBContext context)
        {
            this.context = context;
        }

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

        [HttpPost]
        public IActionResult Delete(int workerId)
        {
            var worker = context.iCAREWorkers.Find(workerId);
            var user = context.iCAREUsers.Find(worker.UserAccount);

            context.iCAREWorkers.Remove(worker);
            context.iCAREUsers.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Manage");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
