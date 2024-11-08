using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Group9_iCareApp.Controllers
{
    public class DisplayMyBoardController : Controller
    {
        private readonly iCAREDBContext _context;
        UserManager<iCAREUser> _userManager;

        public DisplayMyBoardController(iCAREDBContext context, UserManager<iCAREUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Sorts the list of patients based on the sortOrder to be viewed in the view.
        public IActionResult Index(string sortOrder)
        {
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["BirthDateSortParm"] = sortOrder == "birthdate_asc" ? "birthdate_desc" : "birthdate_asc";

            // Track current sort column and direction
            ViewData["CurrentSortColumn"] = sortOrder?.Split('_')[0] ?? "name"; //Default to name if null
            ViewData["CurrentSortDirection"] = sortOrder?.Split('_')[1] ?? "desc"; // Default to ascending if null

            string userID = _userManager.GetUserId(User) ?? string.Empty;
            if (!userID.IsNullOrEmpty())
            {
                iCAREWorker worker = _context.iCAREWorkers.FirstOrDefault(w => w.UserAccount == userID);
                var patients = _context.PatientRecords.Where(c => c.TreatmentRecords.Any(i => i.WorkerId == worker.Id)).AsQueryable();

                patients = sortOrder switch
                {
                    "name_desc" => patients.OrderByDescending(d => d.Fname),
                    "name_asc" => patients.OrderBy(d => d.Fname),
                    "birthdate_desc" => patients.OrderByDescending(d => d.DateOfBirth),
                    "birthdate_asc" => patients.OrderBy(d => d.DateOfBirth),
                    _ => patients.OrderByDescending(d => d.Fname) // Default sort by firstname descending
                };
                ViewData["patients"] = patients.ToList();
            }
            return View();
        }

        //Shows the details of a specific patient based on their id.
        public IActionResult Details(int patientId)
        {
            var patient = _context.PatientRecords.Find(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Patient cannot be found.";
            }
            return View(patient);
        }

        //Given the patient(based on Id), manage the patient's information to change if needed.
        public IActionResult ManagePatient(int patientId)
        {
            DbSet<PatientRecord> allRecords = _context.PatientRecords;
            PatientRecord patient = allRecords.Find(patientId);  // Find the requested patient
            ViewData["bloodtypes"] = iCAREBoardController.bloodGroups;
            return View(patient);
        }

        //Given the patient, update it within the database and go back to myBoard.
        [HttpPost]
        public ActionResult Edit(PatientRecord patient)
        {
            DbSet<PatientRecord> allRecords = _context.PatientRecords;
            allRecords.Update(patient);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Successfully modified " + patient.Fname + " " + patient.Lname + "'s information.";
            return RedirectToAction("Index");
        }

        // Initializes given data for a new treatment record, but then sends to the view to get the rest to treat a patient.
        public IActionResult TreatPatient(int patientId)
        {
            ViewData["Drugs"] = new SelectList(_context.DrugsDictionaries, "Id", "Name");
            string userID = _userManager.GetUserId(User) ?? string.Empty;
            if (!userID.IsNullOrEmpty())
            {
                iCAREWorker worker = _context.iCAREWorkers.FirstOrDefault(w => w.UserAccount == userID);
                if (worker != null)
                {
                    ViewData["WorkerId"] = worker.Id; //if this fails, the view will know about it
                }
            }
            // Initialize the model with today's date for TreatmentDate
            var treatmentRecord = new TreatmentRecord
            {
                TreatmentId = Guid.NewGuid().ToString(),
                TreatmentDate = DateTime.Now,
                PatientId = patientId,
            };
            return View(treatmentRecord);
        }

        //Having created a new treatmentRecord, checks if valid and saves to database. Success = details of patient, showing new record.
        // Failure = go back to treatPatient view.
        [HttpPost]
        public IActionResult CreateTreatmentRecord(TreatmentRecord treatmentRecord)
        {
            if (ModelState.IsValid)
            {
                _context.TreatmentRecords.Add(treatmentRecord);
                _context.SaveChanges();
                return RedirectToAction("Details", new { patientId = treatmentRecord.PatientId });
            }
            ViewData["ErrorMessage"] = "Sorry, treating a patient failed! If this occurs again, contact help!"; //shouldn't ever occur.
            return RedirectToAction("TreatPatient", new { patientId = treatmentRecord.PatientId });
        }
    }
}
