using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group9_iCareApp.Controllers
{
    public class ManagePatientRecordController : Controller
    {
        private readonly iCAREDBContext _usercontext;

        public ManagePatientRecordController(iCAREDBContext usercontext)
        {
            _usercontext = usercontext;
        }


        // GET: ManagePatientRecord?id={id}
        public IActionResult Index(int id)
        {
            DbSet<PatientRecord> allRecords = _usercontext.PatientRecords;
            PatientRecord patient = allRecords.Find(id);  // Find the requested patient
            ViewData["Message"] = patient;  // Pass the patient to the view
            return View();
        }

        // GET: ManagePatientRecord/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagePatientRecord/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManagePatientRecord/Create
        [HttpPost]
        public IActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ManagePatientRecord/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagePatientRecord/Edit?id={id}
        [HttpPost]
        public ActionResult Edit(int id, PatientRecord patient)
        {
            try
            {
                patient.Id = id;
                DbSet<PatientRecord> allRecords = _usercontext.PatientRecords;
                allRecords.Update(patient);
                _usercontext.SaveChanges();
                patient.Id = id;
                ViewData["success"] = true;
                return View();
            }
            catch
            {
                ViewData["success"] = false;
                return View();
            }
        }

        // GET: ManagePatientRecord/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagePatientRecord/Delete/5
        [HttpPost]
        public IActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
