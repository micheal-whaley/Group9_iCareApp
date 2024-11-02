using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Group9_iCareApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly iCAREDBContext _usercontext;

        public HomeController(ILogger<HomeController> logger, iCAREDBContext usercontext)
        {
            _logger = logger;
            _usercontext = usercontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterPage()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult LoginForm(iCAREUser model)
        {
            // Login for administrators
            if (model.Id == null) // doesnt exist yet
            {
                _usercontext.iCAREUsers.Add(model);
            }
            else
            {
               // _context.Expenses.Update(model);
            }
            _usercontext.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
