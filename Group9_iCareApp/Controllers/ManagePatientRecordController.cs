using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Group9_iCareApp.Controllers
{
    public class ManagePatientRecordController : Controller
    {
        // GET: ManagePatientRecord
        public IActionResult Index()
        {
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

        // POST: ManagePatientRecord/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
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
