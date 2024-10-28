using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Group9_iCareApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       // private readonly UserDBContext _usercontext;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
            if (model.Id == 0) // doesnt exist yet
            {
               // _context.Expenses.Add(model);
            }
            else
            {
               // _context.Expenses.Update(model);
            }
           // _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
