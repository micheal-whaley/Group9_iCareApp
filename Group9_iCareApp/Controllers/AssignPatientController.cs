using System;
using System.Collections.Generic;
using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Group9_iCareApp.Controllers
{
    public class AssignPatientController : Controller // anything that is commented out is that way to avoid errors for now until the models are fully implemented
    {
        // GET: Retrieve all patient records

        //public IActionResult RetrieveAllPatients()
        //{
        //    using (var context = new iCAREEntities())
        //    {
        //        var patients = context.PatientRecords.ToList(); // Retrieve all patients from the database
        //        return Json(patients, JsonRequestBehavior.AllowGet); // Return patients as JSON didn't know you could do this until I looked it up.
        //    }
        //}

        // POST: Assign patients to a worker
        //public IActionResult AssignPatients(string workerID, List<string> selectedPatientIDs)
        //{
        //    using (var context = new iCAREEntities())
        //    {
        //        var worker = context.iCAREWorkers.FirstOrDefault(w => w.ID == workerID); // Find the worker by ID
        //        if (worker == null)
        //        {
        //            return Content("Worker not found.");
        //        }

        //        var assignedMessages = new List<string>();

        //        foreach (var patientID in selectedPatientIDs)
        //        {
        //            var patient = context.PatientRecords.FirstOrDefault(p => p.ID == patientID); // Find the patient by ID
        //            if (patient == null)
        //            {
        //                assignedMessages.Add($"Patient with ID {patientID} not found.");
        //                continue;
        //            }

        //            // Use the existing method to assign the nurse
        //            string message = patient.AssignNurse(worker); // Call the AssignNurse method from PatientRecord
        //            assignedMessages.Add(message); // Collect the response message
        //        }

        //        return Json(assignedMessages); // Return the messages as JSON
        //    }
        //}
    }
}
