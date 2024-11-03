using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using IronPdf;
using Group9_iCareApp.Models;
using Group9_iCareApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Group9_iCareApp.Controllers
{
    public class ManageDocumentController : Controller
    {

        private readonly iCAREDBContext _context = new();
        private PDFConverter _converter = new();

        private string[] permittedFileExtensions = { ".png",".jpg", ".pdf" };

        //public ManageDocumentController(DocContext context)
        //{
        //    this._context = context;
        //}

        // GET: ManageDocument
        public IActionResult Index()
        {
            return View();
        }

        // GET: ManageDocument/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageDocument/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageDocument/Create
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

        // GET: ManageDocument/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageDocument/Edit/5
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


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0) //file.ContentType.Contains()
            {



                // Get file details

                var streamedFileContent = Array.Empty<byte>();

                //using (var memoryStream = new MemoryStream())
                //{
                //    await FileUpload.FormFile
                //}
                //streamedFileContent = await FileHelpers.

                string fileName = Path.GetFileName(file.FileName);
                //permittedFileExtensions.Contains()
                //image.Filename = fileName;

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

                // Save the file
                string html;

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose))
                {
                    await file.CopyToAsync(stream);

                    PdfDocument pdf = ImageToPdfConverter.ImageToPdf(filePath);
                    html = pdf.ToHtmlString();
                }

                Document doc = new();
                doc.DocumentName = fileName;
                doc.Html = html;

                if (ModelState.IsValid)
                {
                    _context.Documents.Add(doc);
                    _context.SaveChanges();
                }

                return Ok(file);

                //{

                //}

            }
            return Ok(file);
            return RedirectToAction("Index");


        }
        // GET: ManageDocument/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageDocument/Delete/5
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
