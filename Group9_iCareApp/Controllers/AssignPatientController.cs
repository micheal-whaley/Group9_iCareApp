using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

public class AssignPatientController : Controller
{
    private readonly iCAREDBContext _context;
    private readonly ILogger<AssignPatientController> _logger;

    public AssignPatientController(iCAREDBContext context, ILogger<AssignPatientController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index(string? workerEmail)
    {
        ViewBag.WorkerEmail = workerEmail;

        var patientsQuery = _context.PatientRecords
            .Include(p => p.Location)
            .Include(p => p.TreatmentRecords)
                .ThenInclude(t => t.Worker)
            .GroupBy(p => p.Location);

        var patients = await patientsQuery.ToListAsync();

        if (!string.IsNullOrEmpty(workerEmail))
        {
            var worker = await _context.iCAREWorkers
                .Include(w => w.AccountNavigation)
                .FirstOrDefaultAsync(w => w.AccountNavigation.Email == workerEmail);

            ViewBag.CurrentWorker = worker;
        }

        return View(patients);
    }

    public IActionResult CreatePatient()
    {
        var locations = _context.Locations.ToList(); // Get the list of locations
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
        ViewData["Locations"] = new SelectList(locations, "Id", "Name"); // Pass them as a SelectList
        ViewData["BloodGroups"] = new SelectList(bloodGroups, "Value", "Text");
        return View();
    }


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


    [HttpPost]
    public async Task<IActionResult> AssignPatients(List<int> patientIds, string workerEmail)
    {
        if (patientIds == null || !patientIds.Any())
        {
            return BadRequest("No patients selected.");
        }

        try
        {
            var worker = await _context.iCAREWorkers
                .Include(w => w.AccountNavigation)
                .FirstOrDefaultAsync(w => w.AccountNavigation.Email == workerEmail);

            if (worker == null)
            {
                return NotFound("Worker not found");
            }

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
                    !patient.TreatmentRecords.Any(t => t.Worker.Profession == "Nurse"))
                {
                    _logger.LogWarning("Skipping patient {PatientId}: requires nurse before doctor", patientId);
                    continue;
                }

                if (worker.Profession == "Nurse" &&
                    patient.TreatmentRecords.Count(t => t.Worker.Profession == "Nurse") >= 3)
                {
                    _logger.LogWarning("Skipping patient {PatientId}: max nurses assigned", patientId);
                    continue;
                }

                var treatmentRecord = new TreatmentRecord
                {
                    TreatmentId = Guid.NewGuid().ToString(),
                    PatientId = patientId,
                    WorkerId = worker.Id,
                    TreatmentDate = DateTime.UtcNow,
                    Description = $"Initial assignment of {worker.Profession} to patient"
                };

                _context.TreatmentRecords.Add(treatmentRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { workerEmail });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning patients");
            return StatusCode(500, "An error occurred while assigning patients.");
        }
    }


    [HttpPost]
    public async Task<IActionResult> UnassignPatient(int patientId, string workerEmail)
    {
        try
        {
            var worker = await _context.iCAREWorkers
                .Include(w => w.AccountNavigation)
                .FirstOrDefaultAsync(w => w.AccountNavigation.Email == workerEmail);

            if (worker == null)
            {
                return NotFound("Worker not found");
            }

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
