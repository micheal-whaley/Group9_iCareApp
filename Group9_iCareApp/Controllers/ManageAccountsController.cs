using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Group9_iCareApp.Controllers
{
    public class ManageAccountsController : Controller
    {
        // GET: ManageAccounts
        public IActionResult Index()
        {
            return View();
        }

        // GET: ManageAccounts/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageAccounts/Create
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

        // GET: ManageAccounts/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageAccounts/Edit/5
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

        // GET: ManageAccounts/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageAccounts/Delete/5
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
