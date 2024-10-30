using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;

// anything that is commented out is that way to avoid errors for now until the models are fully implemented

namespace Group9_iCareApp.Controllers
{
    public class DisplayMyBoardController : Controller
    {
        private readonly iCAREDBContext _context;

        public DisplayMyBoardController(iCAREDBContext context)
        {
            _context = context;
        }

        // GET: DisplayMyBoard for logged-on worker
        [HttpGet("DisplayMyBoard#{workerID}")]
        public IActionResult Index(int workerID)
        {
            // Fetch data from the database and check if workerID is valid
            var workerExists = CheckWorkerExists(workerID);
            if (!workerExists)
            {
                // Handle the case where the workerID does not exist
                return View("Error", "Worker ID does not exist.");
            }
            // Fetch data from database.
            var myPatients = RetrieveMyPatients(workerID);
            return View(myPatients);
        }
        private bool CheckWorkerExists(int workerID)
        {
            return _context.iCAREWorkers.Any(w => w.Id == workerID);
        }

        // GET: DisplayMyBoard/Details/5
        public IActionResult Details(int patientId)
        {
            var patient = _context.PatientRecords.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        public IActionResult ManagePatient(int patientId)
        {
            return RedirectToAction("Index", "ManagePatient", new { patientID = patientId }); // redirect to christian's managepatientview.
        }


        // Retrieve all patients assigned to the specified worker.
        public List<PatientRecord> RetrieveMyPatients(int workerID)
        {
            var patientIDs = GetPatientIDs(workerID);
            var patients = GetMyPatients(workerID, patientIDs);
            return patients;
        }


        // Get patient IDs assigned to the specified worker.
        private List<int?> GetPatientIDs(int workerID)
        {
            var patientIDs = _context.TreatmentRecords
                .Where(tr => tr.WorkerId == workerID)
                .Select(tr => tr.PatientId)
                .ToList();

            return patientIDs;
        }

        // Get patient records based on IDs and worker ID.
        private List<PatientRecord> GetMyPatients(int workerID, List<int?> patientIDs)
        {
            var myPatients = _context.PatientRecords
                .Where(p => patientIDs.Contains(p.Id) && p.TreatmentRecords.Any(tr => tr.WorkerId == workerID))
                .ToList();

            return myPatients;
        }
    }
}
