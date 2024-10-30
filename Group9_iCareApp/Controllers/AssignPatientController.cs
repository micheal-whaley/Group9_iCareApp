using System;
using System.Collections.Generic;
using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Group9_iCareApp.Controllers{
    public class AssignPatientController : Controller
    {
        // GET: Retrieve all patient records
        public IActionResult RetrieveAllPatients()
        {
            using var context = new iCAREDBContext();
            var patients = context.PatientRecords.ToList(); // Retrieve all patients from the database
            return Json(patients); // Return patients as JSON
        }

        // POST: Assign patients to a worker
        [HttpPost]
        public IActionResult AssignPatients(string workerId, List<string> selectedPatientIDs)
        {
            using var context = new iCAREDBContext();
            
            var worker = context.iCAREWorkers.FirstOrDefault(w => w.Id == workerId);
            if (worker == null)
            {
                return NotFound("Worker not found.");
            }

            var assignedMessages = new List<string>();

            foreach (var patientId in selectedPatientIDs)
            {
                var patient = context.PatientRecords.FirstOrDefault(p => p.Id == patientId);
                if (patient == null)
                {
                    assignedMessages.Add($"Patient with ID {patientId} not found.");
                    continue;
                }
                string message = patient.AssignToNurse(workerId); // Assuming this method exists and returns a message
                assignedMessages.Add(message);
            }
            return Json(assignedMessages); // Return messages as JSON
        }
    }
}
