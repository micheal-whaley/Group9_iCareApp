using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Aspose.Words;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Group9_iCareApp.Controllers
{
    public class ManageDocumentController : Controller
    {

        private readonly iCAREDBContext db = new();
        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".pdf" };



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
        public  ActionResult Palette()
        {
            return View(db.Documents.ToList());
        }

        //[HttpGet]
        //public async Task<ActionResult> Palette(string sortOrder)
        //{
        //    // Fetch documents from the database
        //    var documents = db.Documents.AsQueryable();

        //    // Determine the sort order (ascending or descending by date)
        //    documents = sortOrder == "date_desc"
        //                ? documents.OrderByDescending(d => d.CreationDate)
        //                : documents.OrderBy(d => d.CreationDate);

        //    // Store the current sort order to toggle in the view
        //    ViewData["DateSortOrder"] = sortOrder == "date_desc" ? "date_asc" : "date_desc";

        //    // Return sorted documents to the view
        //    return View(await documents.ToListAsync());
        //    return View(db.Documents.ToList());
        //}

        public ActionResult ManageDocument()
        {
            return RedirectToAction("CreateDocument"); //if someone tries to access directly, just do as if it's a new document.
        }
        public ActionResult CreateDocument()
        {
            ViewData["Document"] = new Group9_iCareApp.Models.Document();
            ViewData["htmlString"] = string.Empty; //empty for new document//
            ViewData["editOldDoc"] = false;
            return View("ManageDocument");
        }

        public ActionResult EditDocument(string fileName)
        {
            var document = db.Documents.Find(fileName);
            if (document == null)
            {
                TempData["ErrorMessage"] = "Document not found";
                return RedirectToAction("UploadDocument"); //displays error message
                //change this
            }
            var htmlString = ConvertPdfToHtml(document.Data);
            ViewData["Document"] = document;
            ViewData["htmlString"] = htmlString;
            ViewData["editOldDoc"] = true;
            return View("ManageDocument");
        }

        //[HttpPost]
        //public ActionResult SaveDocument(string content, string docName, bool editOldDoc)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SaveChanges();
        //    }
        //    // Validate input
        //    if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(docName))
        //    {
        //        return NoContent();
        //    }

        //    // Check if creating a new document and if the document name already exists
        //    if (!editOldDoc && db.Documents.Find(docName + ".pdf") != null)
        //    {
        //        // Document with the same name already exists
        //        return NoContent();
        //    }

        //    // Convert HTML content to PDF
        //    Aspose.Words.Document doc = new();
        //    var builder = new DocumentBuilder(doc);
        //    builder.InsertHtml(content);
        //    using MemoryStream outStream = new();
        //    doc.Save(outStream, SaveFormat.Pdf);
        //    byte[] bytes = outStream.ToArray();

        //    if (ModelState.IsValid)
        //    {
        //        db.SaveChanges();
        //    }

        //    // Handle document saving/updating based on edit mode
        //    //Group9_iCareApp.Models.Document document;
        //    if (editOldDoc)
        //    {
        //        var document = db.Documents.Find(docName);
        //        if (document == null) return NoContent(); // Ensure document exists before updating
        //        if (ModelState.IsValid)
        //        {
        //            document.Data = bytes;
        //            db.SaveChanges();
        //        }
        //        document.Data = bytes;
        //    }
        //    else
        //    {
        //        var document = new Group9_iCareApp.Models.Document
        //        {
        //            DocumentName = docName + ".pdf",
        //            Data = bytes
        //        };
        //        if (ModelState.IsValid)
        //        {
        //            db.Documents.Add(document);
        //            db.SaveChanges();
        //        }
        //    }
        //    // Save changes
            

        //    return RedirectToAction("ManageDocument");
        //}


        [HttpPost]
        public ActionResult SaveNewDocument(string content, string docName)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(docName) || db.Documents.Find(docName + ".pdf") != null) //if already exists
            {
                //nothing in doc, nothing in docName, or doc already exists.
                return NoContent();
            }

            // Convert HTML content to PDF
            Aspose.Words.Document doc = new();
            var builder = new DocumentBuilder(doc);
            builder.InsertHtml(content);
            using MemoryStream outStream = new();
            doc.Save(outStream, SaveFormat.Pdf);
            byte[] bytes = outStream.ToArray();

            Group9_iCareApp.Models.Document document = new()
            {
                DocumentName = docName + ".pdf",
                Data = bytes,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
            }
            return RedirectToAction("Palette");
        }

        [HttpPost]
        public ActionResult SaveOldDocument(string content, string docName)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(docName)) 
            {
                //nothing in doc, nothing in docName.
                return NoContent();
            }

            Aspose.Words.Document doc = new();
            var builder = new DocumentBuilder(doc);
            builder.InsertHtml(content);
            using MemoryStream outStream = new();
            doc.Save(outStream, SaveFormat.Pdf);
            byte[] bytes = outStream.ToArray();

            Group9_iCareApp.Models.Document document = db.Documents.Find(docName);

            if (ModelState.IsValid)
            {
                document.Data = bytes; //update byte data
                db.SaveChanges();
            }
            return RedirectToAction("Palette");
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
            if (file == null || file.Length == 0) 
            {
                TempData["ErrorMessage"] = "Please select a file to upload.";
                return RedirectToAction("UploadDocument"); //File not uploaded.
            }

            var fileNameNoExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (db.Documents.Find(fileNameNoExtension + ".pdf") != null)
            {
                TempData["ErrorMessage"] = "File with that name already exists in the database. Please upload a valid file.";
                return RedirectToAction("UploadDocument"); //File already exists.
            }
            if (!permittedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "File type is not supported. Please upload a valid file.";
                return RedirectToAction("UploadDocument"); //Extension not allowed.
            }

            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                pdfBytes = fileExtension switch
                {
                    ".png" or ".jpg" or ".jpeg" => ConvertImageToPdf(stream),
                    ".doc" or ".docx" => ConvertWordToPdf(stream),
                    ".pdf" => ConvertPdfToBytes(stream),
                    _ => null
                };
            }

            if(pdfBytes == null)
            {
                TempData["ErrorMessage"] = "Failed to process the file. Please try again.";
                return RedirectToAction("UploadDocument"); //This should never happen, but just in case.
            }

            Group9_iCareApp.Models.Document document = new()
            {
                DocumentName = fileNameNoExtension + ".pdf",
                Data = pdfBytes,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Document uploaded successfully!";
            }else
            { 
                TempData["ErrorMessage"] = "Failed to upload the document. Please check the file and try again.";
            }
            return RedirectToAction("UploadDocument");
        }

        private static string ConvertPdfToHtml(byte[] pdfData)
        {
            var options = new Aspose.Pdf.HtmlSaveOptions
            {
                FixedLayout = true,
                PartsEmbeddingMode = Aspose.Pdf.HtmlSaveOptions.PartsEmbeddingModes.EmbedAllIntoHtml,
                RasterImagesSavingMode = Aspose.Pdf.HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground
            };

            using var pdfStream = new MemoryStream(pdfData);
            using var htmlStream = new MemoryStream();
            var pdf = new Aspose.Pdf.Document(pdfStream);
            pdf.Save(htmlStream, options);
            return System.Text.Encoding.UTF8.GetString(htmlStream.ToArray());
        }
        private static byte[] ConvertImageToPdf(Stream imageStream)
        {
            Aspose.Words.Document doc = new();
            var builder = new DocumentBuilder(doc);
            builder.InsertImage(imageStream); //insert image//

            using var outStream = new MemoryStream();
            doc.Save(outStream, SaveFormat.Pdf); //turn into PDF 
            return outStream.ToArray(); 
        }

        private static byte[] ConvertWordToPdf(Stream wordStream)
        {
            Aspose.Words.Document doc = new(wordStream);

            using var outStream = new MemoryStream();
            doc.Save(outStream, SaveFormat.Pdf); //turn into PDF
            return outStream.ToArray();
        }

        private static byte[] ConvertPdfToBytes(Stream pdfStream)
        {
            Aspose.Pdf.Document pdf = new(pdfStream);

            using var outStream = new MemoryStream();
            pdf.Save(outStream);
            return outStream.ToArray();

        }
    }  
}
