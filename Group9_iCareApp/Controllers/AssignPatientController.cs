using System;
using System.Collections.Generic;
using System.Linq;
using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Group9_iCareApp.Controllers
{
    public class AssignPatientController : Controller
    {
        SqlCommand com = new SqlCommand();

        SqlDataReader dr;
        SqlConnection con = new SqlConnection();

        private readonly iCAREDBContext _context;
        private readonly ILogger<AssignPatientController> _logger;
        private const int MAX_NURSES_PER_PATIENT = 3;
        public List<Location> locations = new List<Location>();

        public AssignPatientController(iCAREDBContext context, ILogger<AssignPatientController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            string connectionString = _context.connectionString;
            con.ConnectionString = connectionString;
            FetchData();
        }


        public Microsoft.AspNetCore.Mvc.Rendering.SelectList Locations { get; set; }
        private void FetchData()
        {

            if (locations.Count > 0)
            {
                locations.Clear();
            }

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [ID],[name],[Description] FROM [Group9_iCareDB].[dbo].[Location]";
                dr = com.ExecuteReader();
                Console.WriteLine("RAAAAH\n");
                while (dr.Read())
                {
                    Console.Write("AAAAH LOOPING\n");
                    locations.Add(new Location()
                    {
                        Id = int.Parse(dr["ID"].ToString()),
                        Name = dr["name"].ToString()
                    });
                }
                Locations = new SelectList(locations, "Id", "Name");
                con.Close();

            }
            catch
            {
                throw;
            }
        }


        [HttpGet("patients")]
        public ActionResult<IEnumerable<PatientRecord>> GetAllPatients()
        {
            try
            {
                var patients = _context.PatientRecords
                    .AsNoTracking()
                    .ToList();

                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients");
                return StatusCode(420, "An error occurred while retrieving patients");
            }
        }



        [HttpPost("assign")]
        public ActionResult<AssignmentResponse> AssignPatients(AssignmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var worker = _context.iCAREWorkers
                    .AsNoTracking()
                    .FirstOrDefault(w => w.Id == request.WorkerId);

                if (worker == null)
                {
                    return NotFound($"Worker with ID {request.WorkerId} not found");
                }

                var response = new AssignmentResponse
                {
                    Messages = new List<string>()
                };

                foreach (var patientId in request.PatientIds)
                {
                    string message = worker.Profession switch
                    {
                        "Nurse" => AssignNurseToPatient(patientId, request.WorkerId),
                        "Doctor" => AssignDoctorToPatient(patientId, request.WorkerId),
                        _ => $"Invalid profession: {worker.Profession}"
                    };
                    response.Messages.Add(message);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning patients to worker {WorkerId}", request.WorkerId);
                return StatusCode(500, "An error occurred while assigning patients");
            }
        }

        public string AssignNurseToPatient(int patientId, int nurseId)
        {
            var patient = _context.PatientRecords
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == patientId);

            if (patient == null)
            {
                return $"Assignment failed: Patient with ID {patientId} does not exist.";
            }

            var nurseCount = _context.TreatmentRecords
                .Count(p => p.PatientId == patientId &&
                           _context.iCAREWorkers
                                .Any(w => w.Id == p.WorkerId && w.Profession == "Nurse"));

            if (nurseCount >= MAX_NURSES_PER_PATIENT)
            {
                return $"Assignment failed: Maximum of {MAX_NURSES_PER_PATIENT} nurses already assigned to patient {patientId}.";
            }

            var treatmentRecord = new TreatmentRecord
            {
                TreatmentId = Guid.NewGuid().ToString(),
                PatientId = patientId,
                WorkerId = nurseId,
                TreatmentDate = DateTime.UtcNow
            };

            _context.TreatmentRecords.Add(treatmentRecord);
            _context.SaveChanges();

            return $"Assignment successful: Nurse {nurseId} assigned to patient {patientId}.";
        }

        public IActionResult AssignPatient()
        {
            var patients = _context.PatientRecords.AsNoTracking().ToList();
            return View(patients); // This will pass the list of patients to the view
        }

        public IActionResult CreatePatient()
        {
            ViewData["Locations"] = Locations;
            return View();
        }


        [HttpPost]
        public ActionResult CreatePatient(int id, PatientRecord patient)
        {

            var newPatient = CreateRecord();

            newPatient.Id = id;
            newPatient.Fname = patient.Fname;
            newPatient.Lname = patient.Lname;
            newPatient.Address = patient.Address;
            newPatient.DateOfBirth = patient.DateOfBirth;
            newPatient.Height = patient.Height;
            newPatient.Weight = patient.Weight;
            newPatient.BloodGroup = patient.BloodGroup;
            newPatient.BedId = patient.BedId;
            newPatient.TreatmentArea = patient.TreatmentArea;
            newPatient.LocationId = patient.LocationId;

            DbSet<PatientRecord> allRecords = _context.PatientRecords;
            allRecords.Add(newPatient);
            _context.SaveChanges();




            return View();
        }


        private PatientRecord CreateRecord()
        {
            try
            {
                return Activator.CreateInstance<PatientRecord>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(iCAREUser)}'. " +
                    $"Ensure that '{nameof(iCAREUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private string AssignDoctorToPatient(int patientId, int doctorId)
        {
            var hasNurse = _context.TreatmentRecords
                .Any(tr => tr.PatientId == patientId &&
                          _context.iCAREWorkers
                               .Any(w => w.Id == tr.WorkerId && w.Profession == "Nurse"));

            if (!hasNurse)
            {
                return $"Assignment failed: No nurse assigned to patient {patientId}.";
            }

            var treatmentRecord = new TreatmentRecord
            {
                TreatmentId = Guid.NewGuid().ToString(),
                PatientId = patientId,
                WorkerId = doctorId,
                TreatmentDate = DateTime.UtcNow
            };

            _context.TreatmentRecords.Add(treatmentRecord);
            _context.SaveChanges();

            return $"Assignment successful: Doctor {doctorId} assigned to patient {patientId}.";
        }
    }

    public class AssignmentRequest
    {
        public int WorkerId { get; set; }
        public List<int> PatientIds { get; set; } = new List<int>();
    }

    public class AssignmentResponse
    {
        public List<string> Messages { get; set; } = new List<string>();
    }
}