using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group9_iCareApp.Controllers
{
    public class AssignPatientController : Controller
    {
        private readonly iCAREDBContext _context;

        public AssignPatientController(iCAREDBContext context)
        {
            _context = context;
        }

        // Action to display all patient records
        public async Task<IActionResult> Index()
        {
            var patientRecords = await _context.PatientRecords
                .Include(p => p.Location)  // You can include other related entities if necessary
                .ToListAsync();

            return View(patientRecords);
        }
    }
} 
