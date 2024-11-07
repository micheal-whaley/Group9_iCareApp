using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Aspose.Words;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Group9_iCareApp.Controllers
{
    public class ManageDocumentController : Controller
    {

        private readonly iCAREDBContext db = new();
        private readonly UserManager<iCAREUser> userManager;
        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".pdf" };

        public ManageDocumentController(UserManager<iCAREUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Palette");
        }

        public IActionResult ManageDocument()
        {
            return RedirectToAction("CreateDocument"); //if someone tries to access directly, just do as if it's a new document.
        }

        [HttpGet]
        public async Task<IActionResult> Palette(string sortOrder)
        {
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["LastModifiedSortParm"] = sortOrder == "modified_asc" ? "modified_desc" : "modified_asc";
            ViewData["CreationSortParm"] = sortOrder == "creation_asc" ? "creation_desc" : "creation_asc";

            // Track current sort column and direction
            ViewData["CurrentSortColumn"] = sortOrder?.Split('_')[0] ?? "name"; //Default to name if null
            ViewData["CurrentSortDirection"] = sortOrder?.Split('_')[1] ?? "desc"; // Default to ascending if null

            // Fetch documents
            var documents = db.Documents.AsQueryable();

            documents = sortOrder switch
            {
                "name_desc" => documents.OrderByDescending(d => d.DocumentName),
                "name_asc" => documents.OrderBy(d => d.DocumentName),
                "modified_desc" => documents.OrderByDescending(d => d.LastModifiedDate),
                "modified_asc" => documents.OrderBy(d => d.LastModifiedDate),
                "creation_desc" => documents.OrderByDescending(d => d.CreationDate),
                "creation_asc" => documents.OrderBy(d => d.CreationDate),
                _ => documents.OrderByDescending(d => d.DocumentName) // Default sort by DocumentName descending
            };
            
            return View(await documents.ToListAsync());
        }

        public IActionResult CreateDocument()
        {
            ViewData["Document"] = new Group9_iCareApp.Models.Document();
            ViewData["htmlString"] = string.Empty; //empty for new document//
            ViewData["editOldDoc"] = false;
			return View("ManageDocument");
        }

        public IActionResult EditDocument(string fileName)
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

        public IActionResult ViewDocument(string fileName)
        {
            var document = db.Documents.Find(fileName);
            ViewData["Document"] = document;
            return View();
        }

        public IActionResult ViewPdf(string fileName)
        {
            var document = db.Documents.Find(fileName);
            return File(document.Data, "application/pdf");
        }

        public IActionResult DeleteDocument(string fileName)
        {
            var doc = db.Documents.Find(fileName);
            if (doc != null && ModelState.IsValid)
            {
                db.Documents.Remove(doc);
                db.SaveChanges();
            }
            return RedirectToAction("Palette");
        }

        [HttpPost]
        public IActionResult SaveNewDocument(string content, string docName)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(docName) || db.Documents.Find(docName + ".pdf") != null) 
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


            int workerID = db.iCAREWorkers.FirstOrDefault(w => w.UserAccount == userManager.GetUserId(User)).Id;
            Group9_iCareApp.Models.Document document = new()
            {
                DocumentName = docName + ".pdf",
                Data = bytes,
                CreationDate = DateTime.Now,
                CreatingWorkerId = workerID,
                LastModifiedDate = DateTime.Now,
                ModifyingWorkerId = workerID,
                //need patient + description
            };

            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
            }
            return RedirectToAction("Palette");
        }

        [HttpPost]
        public IActionResult SaveOldDocument(string content, string docName)
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

            if(document == null)
            {
                return NoContent(); //doc can't be found
            }

            if (ModelState.IsValid)
            {
                document.Data = bytes; //update byte data
                document.LastModifiedDate = DateTime.Now;
                document.ModifyingWorkerId = db.iCAREWorkers.FirstOrDefault(w => w.UserAccount == userManager.GetUserId(User)).Id;
                db.SaveChanges();
            }
            return RedirectToAction("Palette");
        }

        public IActionResult UploadDocument()
        {
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

            int workerID = db.iCAREWorkers.FirstOrDefault(w => w.UserAccount == userManager.GetUserId(User)).Id;
            Group9_iCareApp.Models.Document document = new()
            {
                DocumentName = fileNameNoExtension + ".pdf",
                Data = pdfBytes,
                CreationDate = DateTime.Now,
                CreatingWorkerId = workerID,
                LastModifiedDate = DateTime.Now,
                ModifyingWorkerId = workerID,
                //patientID + description
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
