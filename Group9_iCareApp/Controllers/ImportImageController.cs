using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Group9_iCareApp.Controllers
{
    public class ImportImageController : Controller
    {
        // GET: ImportImage
        public IActionResult Index()
        {
            return View();
        }

        // GET: ImportImage/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportImage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImportImage/Create
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

        // GET: ImportImage/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImportImage/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, FormCollection collection)
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

        // GET: ImportImage/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImportImage/Delete/5
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
