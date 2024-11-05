using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Aspose.Words;
using System.IO;

namespace Group9_iCareApp.Controllers
{
    public class ManageDocumentController : Controller
    {

        private readonly iCAREDBContext db = new();

        private readonly string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
        private readonly string[] docExtensions = { ".doc", ".docx" };
        private readonly string[] pdfExtensions = { ".pdf" };
        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".pdf" };
        private readonly string baseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");



        // GET: ManageDocument
        public IActionResult Index()
        {
            return RedirectToAction("Palette");
        }

        // GET: ManageDocument/Details/5
        //public IActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ManageDocument/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: ManageDocument/Create
        [HttpPost]
        //public IActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ManageDocument/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ManageDocument/Edit/5
        //[HttpPost]
        //public IActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}




        //// GET: ManageDocument/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ManageDocument/Delete/5
        //[HttpPost]
        //public IActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        [HttpGet]
        public ActionResult Palette()
        {
            return View(db.Documents.ToList());
        }

        public ActionResult CreateDocument()
        {
            return View();
        }

        public IActionResult ViewDocument(string fileName)
        {
            var document = db.Documents.Find(fileName);
            ViewData["Document"] = document;
            return View();
        }


        public ActionResult ViewPdf(string fileName)
        {
            var document = db.Documents.Find(fileName);
            if (document == null)
            {
                return NotFound();
            }
            return File(document.Data, "application/pdf");
        }

        

        public ActionResult UploadDocument()
        {
            ViewData["error"] = ""; //just want it to not be null
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            
            if (file != null && file.Length > 0) //basic file checks
            {

                var fileName = file.FileName;
                var startIndex = 0;
                var smallfileName = fileName.Substring(startIndex, fileName.LastIndexOf('.'));

                if (db.Documents.Find(smallfileName + ".pdf") != null)
                {
                    ViewData["error"] = "repeat";
                    return NoContent(); //file already exists
                }
                byte[] bytes = null;
                foreach (var extension in permittedExtensions)
                {
                    if (fileName.LastIndexOf(extension) != -1) //as long as this extension exists
                    {
                        Directory.CreateDirectory(baseFilePath);
                        string filePath = Path.Combine(baseFilePath, fileName);
                        string pdfFilePath = Path.Combine(baseFilePath, smallfileName + ".pdf");
                        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 1000000, FileOptions.None);

                        await file.CopyToAsync(stream);
                        stream.Dispose();

                        if (imageExtensions.Contains(extension))
                        {
                            Aspose.Words.Document doc = new();
                            var builder = new DocumentBuilder(doc);
                            builder.InsertImage(filePath); //insert image//
                            MemoryStream outStream = new();
                            doc.Save(outStream, SaveFormat.Pdf);
                            bytes = outStream.ToArray(); //get byte data//
                            outStream.Dispose(); //free memory
                        }
                        else if (docExtensions.Contains(extension))
                        {
                            Aspose.Words.Document doc = new(filePath);
                            MemoryStream outStream = new();
                            doc.Save(outStream, SaveFormat.Pdf);
                            bytes = outStream.ToArray();
                            outStream.Dispose();
                        }
                        else if (pdfExtensions.Contains(extension))
                        {
                            Aspose.Pdf.Document pdf = new(filePath);
                            MemoryStream outStream = new();
                            pdf.Save(outStream);
                            bytes = outStream.ToArray();
                            outStream.Dispose();
                        }

                        System.IO.File.Delete(filePath); //delete temp file
                        Directory.Delete(baseFilePath); //deleet temp directory
                        break;
                    }
                }
                if (bytes == null) //if none of the allowed extensions were found.
                {
                    return NoContent();
                }
                Group9_iCareApp.Models.Document document = new()
                {
                    DocumentName = smallfileName + ".pdf",
                    Data = bytes
                };

                if (ModelState.IsValid)
                {
                    db.Documents.Add(document);
                    db.SaveChanges();
                }
                return RedirectToAction("UploadDocument");
            }
            return NoContent();
        }
        
    }  
}
