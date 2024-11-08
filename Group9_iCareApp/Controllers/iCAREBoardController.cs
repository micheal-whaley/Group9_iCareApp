using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class iCAREBoardController : Controller
{
    private readonly iCAREDBContext _context;
    private readonly ILogger<iCAREBoardController> _logger;


    public static List<SelectListItem> bloodGroups = new List<SelectListItem>
        {
            new SelectListItem { Text = "A+", Value = "A+" },
            new SelectListItem { Text = "A-", Value = "A-" },
            new SelectListItem { Text = "B+", Value = "B+" },
            new SelectListItem { Text = "B-", Value = "B-" },
            new SelectListItem { Text = "AB+", Value = "AB+" },
            new SelectListItem { Text = "AB-", Value = "AB-" },
            new SelectListItem { Text = "O+", Value = "O+" },
            new SelectListItem { Text = "O-", Value = "O-" }
        };
    // Constructor with dependency injection for DbContext and Logger
    public iCAREBoardController(iCAREDBContext context, ILogger<iCAREBoardController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Display list of patients assigned to worker by location
    public async Task<IActionResult> Index()
    {
        // Query to fetch patients grouped by location, including treatment records and worker details
        var patientsQuery = _context.PatientRecords
            .Include(p => p.Location)
            .Include(p => p.TreatmentRecords)
                .ThenInclude(t => t.Worker)
            .GroupBy(p => p.Location);

        var patients = await patientsQuery.ToListAsync();

        return View(patients);
    }

    // GET: Render Create Patient form with blood group and location options
    public IActionResult CreatePatient()
    {

        var bloodGroups = new List<SelectListItem>
        {
            new SelectListItem { Text = "A+", Value = "A+" },
            new SelectListItem { Text = "A-", Value = "A-" },
            new SelectListItem { Text = "B+", Value = "B+" },
            new SelectListItem { Text = "B-", Value = "B-" },
            new SelectListItem { Text = "AB+", Value = "AB+" },
            new SelectListItem { Text = "AB-", Value = "AB-" },
            new SelectListItem { Text = "O+", Value = "O+" },
            new SelectListItem { Text = "O-", Value = "O-" }
        };

        var locations = _context.Locations.ToList(); // Get the list of locations
        ViewData["Locations"] = new SelectList(locations, "Id", "Name"); // Pass them as a SelectList
        ViewData["BloodGroups"] = new SelectList(bloodGroups, "Value", "Text");
        return View();
    }

    // POST: Create a new patient record and save to the database
    [HttpPost]
    public async Task<IActionResult> CreatePatient(int id, PatientRecord patient)
    {
        var newPatient = new PatientRecord
        {
            Id = id,
            Fname = patient.Fname,
            Lname = patient.Lname,
            Address = patient.Address,
            DateOfBirth = patient.DateOfBirth,
            Height = patient.Height,
            Weight = patient.Weight,
            BloodGroup = patient.BloodGroup,
            BedId = patient.BedId,
            TreatmentArea = patient.TreatmentArea,
            LocationId = patient.LocationId
        };

        _context.PatientRecords.Add(newPatient);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // POST: Assign multiple patients to a specified worker
    [HttpPost]
    public async Task<IActionResult> AssignPatients(List<int> patientIds, string userid)
    {
        if (!patientIds.Any())
        {
            TempData["ErrorMessage"] = "No patients were selected to be assigned.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // Select worker from DB
            var worker = await _context.iCAREWorkers
                .FirstOrDefaultAsync(w => w.UserAccount == userid);

            var user = await _context.iCAREUsers
                .FirstOrDefaultAsync(u => u.Id == userid);

            if (worker == null || user == null)
                return NotFound("Worker not found");

            foreach (var patientId in patientIds)
            {
                var patient = await _context.PatientRecords
                    .Include(p => p.TreatmentRecords)
                        .ThenInclude(t => t.Worker)
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                {
                    _logger.LogWarning("Patient {PatientId} not found, skipping", patientId);
                    continue;
                }

                if (worker.Profession == "Doctor" &&
                    (patient.TreatmentRecords.All(t => t.Worker?.Profession != "Nurse") ||
                     patient.TreatmentRecords.Count(t => t.Worker?.Profession == "Doctor") >= 1))
                {
                    TempData["ErrorMessage"] = "A doctor cannot be assigned without a nurse assigned first, or only one doctor is allowed.";
                    _logger.LogWarning("Skipping patient {PatientId}: requires nurse before doctor", patientId);
                    continue;
                }

                if (patient.TreatmentRecords.Any(t => t.WorkerId == worker.Id))
                {
                    TempData["ErrorMessage"] = "Worker is already assigned to patient.";
                    _logger.LogWarning("Skipping patient {PatientId}: worker already assigned", patientId);
                    continue;
                }

                if (worker.Profession == "Nurse" &&
                    patient.TreatmentRecords.Count(t => t.Worker?.Profession == "Nurse") >= 3)
                {
                    TempData["ErrorMessage"] = "Maximum of 3 nurses per patient reached.";
                    _logger.LogWarning("Skipping patient {PatientId}: max nurses assigned", patientId);
                    continue;
                }

                var treatmentRecord = new TreatmentRecord
                {
                    TreatmentId = Guid.NewGuid().ToString(),
                    PatientId = patientId,
                    WorkerId = worker.Id,
                    TreatmentDate = DateTime.UtcNow,
                    Description = $"Initial assignment of {worker.Profession} {user.Fname} {user.Lname} to patient"
                };

                _context.TreatmentRecords.Add(treatmentRecord);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Patient(s) have been successfully assigned.";
            return RedirectToAction(nameof(Index), new { userid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning patients");
            return StatusCode(500, "An error occurred while assigning patients.");
        }
    }

    // POST: Unassign a patient from a specified worker
    [HttpPost]
    public async Task<IActionResult> UnassignPatient(int patientId, string workerEmail)
    {
        try
        {
            var worker = await _context.iCAREWorkers
                .Include(w => w.AccountNavigation)
                .FirstOrDefaultAsync(w => w.AccountNavigation.Email == workerEmail);

            if (worker == null)
                return NotFound("Worker not found");

            var treatmentRecord = await _context.TreatmentRecords
                .FirstOrDefaultAsync(t => t.PatientId == patientId && t.WorkerId == worker.Id);

            if (treatmentRecord != null)
            {
                _context.TreatmentRecords.Remove(treatmentRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { workerEmail });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unassigning patient {PatientId}", patientId);
            return StatusCode(500, "An error occurred while unassigning the patient.");
        }
    }
}