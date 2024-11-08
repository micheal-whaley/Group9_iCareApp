using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Group9_iCareApp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index() // shows the main view telling the user to login
        {
            return View();
        }

        public IActionResult AboutUs() // shows the about us page with information about iCare
        {
            return View();
        }

        public IActionResult Help() // shows the help page with contact information
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() // shows the error page incase there is a problem with rendering
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}