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
        public async Task<IActionResult> Update(string accountGUID, 
                                    string fname, 
                                    string lname, 
                                    int location)
        {
            // Takes the GUID and updates the iCAREUser associated with it
            iCAREUser? user = context.iCAREUsers.Find(accountGUID);

            // Doesn't exist
            if (user == null)
            {
                Redirect("~/Views/Shared/Error");
            }

            // update the user
            user.Fname = fname;
            user.Lname = lname;
            user.locationID = location;
            context.iCAREUsers.Update(user);    // Update the record R where R.Id == User.Id

            await context.SaveChangesAsync();  // Save the changes to the instance of the DB
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
