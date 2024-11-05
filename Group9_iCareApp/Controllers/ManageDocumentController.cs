using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Aspose.Words;

namespace Group9_iCareApp.Controllers
{
    public class ManageDocumentController : Controller
    {

        private readonly iCAREDBContext _context = new();

        private readonly string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
        private readonly string[] docExtensions = { ".doc", ".docx" };
        private readonly string[] pdfExtensions = { ".pdf" };
        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".pdf" };
        private readonly string baseFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "uploads");



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
            Directory.CreateDirectory(baseFilePath);
            if (file != null && file.Length > 0) //basic file checks
            {

                var fileName = file.FileName;
                var smallfileName = fileName.Substring(0, fileName.LastIndexOf("."));

                if(_context.Documents.Find(smallfileName+".pdf") != null)
                {
                    return NoContent(); //file already exists
                }
                byte[] bytes = null;
                foreach (var extension in permittedExtensions)
                {
                    if (fileName.LastIndexOf(extension) != -1) //as long as this extension exists
                    {
                        string filePath = System.IO.Path.Combine(baseFilePath, fileName);
                        string pdfFilePath = System.IO.Path.Combine(baseFilePath, smallfileName + ".pdf");
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
                        break;
                    }

                }
                 
                if(bytes == null) //if none of the allowed extensions were found.
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
                    _context.Documents.Add(document);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
                //return RedirectToAction("ViewDocument");
            }
            return NoContent();
            //return RedirectToAction("Index");
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

        public IActionResult ViewDocument(string fileName)
        {
            var document = _context.Documents.Find(fileName);
            ViewData["Document"] = document;
            return View();
        }


        public ActionResult ViewPdf(string fileName)
        {
            var document = _context.Documents.Find(fileName);
            if (document == null)
            {
                return NotFound();
            }
            return File(document.Data, "application/pdf");
        }

        public ActionResult Palette()
        {
            return View(_context.Documents.ToList());
        }

    }
    
}
