using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Group9_iCareApp.Models;

namespace Group9_iCareApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly iCAREDBContext context;

        // This controller is only here to allow for dependency injection
        public AdminController(iCAREDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Create()
        {
            return Redirect("~/Identity/Account/Register");
        }

        public IActionResult Update(int workerId)
        {
            // Renders the actual page.
            ViewBag.workerId = workerId;
            ViewData["context"] = context;
            return View();
        }

        [HttpPost]
        public IActionResult Update(int workerId, 
                                    string fname, string lname, 
                                    string profession, int location)
        {
            // Takes the workerId and updates the iCAREWorker and iCAREUser associated with it
            iCAREWorker? worker = context.iCAREWorkers.Find(workerId);
            iCAREUser? user = context.iCAREUsers.Find(worker?.UserAccount ?? string.Empty);

            // Doesn't exist
            if (user == null || worker == null)
            {
                Redirect("~/Views/Shared/Error");
            }
            // Update the worker
            worker.Profession = profession;
            context.iCAREWorkers.Update(worker);
            // update the user
            user.Fname = fname;
            user.Lname = lname;
            user.locationID = location;
            context.iCAREUsers.Update(user);

            context.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpPost]
        public IActionResult Delete(int workerId)
        {
            // Takes a workerId and removes the worker from the database along with their associated account
            var worker = context.iCAREWorkers.Find(workerId);
            var user = context.iCAREUsers.Find(worker.UserAccount);

            // runtime changes
            context.iCAREWorkers.Remove(worker);
            context.iCAREUsers.Remove(user);
            // Reflect changes on actual database server
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
