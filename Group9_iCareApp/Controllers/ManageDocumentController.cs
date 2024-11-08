using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Aspose.Words;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Group9_iCareApp.Controllers
{
    public class ManageDocumentController : Controller
    {

        private readonly iCAREDBContext db = new();
        private readonly UserManager<iCAREUser> userManager;
        //not allowing all extensions to just be uploaded. Covers basic images, docs, and pdfs.
        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".pdf" };

        //injects userManager to allow for checking what User is currently here.
        public ManageDocumentController(UserManager<iCAREUser> userManager)
        {
            this.userManager = userManager;
        }
        //makes sure that if someone goes to /ManageDocument/ URL, it redirects it to Palette.
        public IActionResult Index()
        {
            return RedirectToAction("Palette");
        }

        //if someone tries to access /ManageDocument/ManageDocument directly, just redirect them to creating a new document.
        public IActionResult ManageDocument()
        {
            return RedirectToAction("CreateDocument"); 
        }

        //The Palette shows documents that have to do with the patients assigned to the worker. Based on the given inputs, it can sort and filter the documents by patient.
        //The default is by names in alphabetical order.
        [HttpGet]
        public async Task<IActionResult> Palette(string? sortOrder, int? patientId)
        {
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc"; 
            ViewData["LastModifiedSortParm"] = sortOrder == "modified_asc" ? "modified_desc" : "modified_asc";
            ViewData["CreationSortParm"] = sortOrder == "creation_asc" ? "creation_desc" : "creation_asc"; 
            //If it was already sorted by some caegory x ascending or descending, then the next time it is sorted again, is it the other way around.
            //If there was no order, the default is ascending for when the button pressed.

            // Track current sort column and direction
            ViewData["CurrentSortColumn"] = sortOrder?.Split('_')[0] ?? "name"; //Default to name if null
            ViewData["CurrentSortDirection"] = sortOrder?.Split('_')[1] ?? "desc"; // Default to ascending if null
            ViewData["patients"] = await CreatePatientSelectList();

            // Set sort and filter parameters
            ViewData["currentSortOrder"] = sortOrder;
            ViewData["currentPatientId"] = patientId;

            // Fetch documents
            var documents = db.Documents.AsQueryable();
            if (patientId.HasValue) //if we are filtering by a patient.
            {
                 documents = documents.Where(d => d.PatientRecordId == patientId);
            }
            else
            {
                //other wise just get all documents associated with this patient.
                var assignedPatientIds = await FindAssignedPatientIds();
                documents = documents.Where(d => assignedPatientIds.Contains(d.PatientRecordId));
            }
            
            //sorts depending on the sortOrder chosen.
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
            
            //pass the list of documents to be viewed.
            return View(await documents.ToListAsync());
        }

        //Creates a new document, sets up necessary information such as the treatments to select one, and whether this is a new document(important for the view).
        public async Task<IActionResult> CreateDocument()
        {
            ViewData["Document"] = new Group9_iCareApp.Models.Document();
            ViewData["htmlString"] = string.Empty; //empty for new document//
            ViewData["editOldDoc"] = false;
            ViewData["treatments"] = await CreateTreatmentSelectList();
            ViewData["drugs"] = db.DrugsDictionaries.ToList();
            return View("ManageDocument");
        }

        //Editing a document is stil the same view, but finds the original information associated with the document so that it is available to be edited. Requires a fileName to do.
        public async Task<IActionResult> EditDocument(string fileName)
        {
            var document = db.Documents.Find(fileName);
            if (document == null)
            {
                return RedirectToAction("CreateDocument"); //can't find document, so just attempts to make new one instead.
            }
            var htmlString = ConvertPdfToHtml(document.Data);
            ViewData["Document"] = document;
            ViewData["htmlString"] = htmlString;
            ViewData["editOldDoc"] = true;
            ViewData["treatments"] = await CreateTreatmentSelectList();
            ViewData["drugs"] = db.DrugsDictionaries.ToList();
            return View("ManageDocument");
        }

        //Views a document given the fileName. Shows associated metadata in it.
        public IActionResult ViewDocument(string fileName)
        {

            var document = db.Documents.Find(fileName);
            if (document != null)
            {   
                //attempts to find the names of the Creator/Last Modifier to be displayed to the view.
                var creatingWorker = db.iCAREWorkers.Find(document.CreatingWorkerId);
                if(creatingWorker != null)
                {
                    var creatingUser = db.iCAREUsers.Find(creatingWorker.UserAccount);
                    if(creatingUser != null)
                    {
                        ViewData["CreatorName"] = creatingUser.Fname + " " + creatingUser.Lname;
                    }
                }

                var modifyingWorker = db.iCAREWorkers.Find(document.ModifyingWorkerId);
                if (modifyingWorker != null)
                {
                    var modifyingUser = db.iCAREUsers.Find(modifyingWorker.UserAccount);
                    if (modifyingUser != null)
                    {
                        ViewData["ModifierName"] = modifyingUser.Fname + " " + modifyingUser.Lname;
                    }
                }
            }

            return View(document);
        }

        //returns the data associated with the document to be viewed as a PDF for the worker.
        public IActionResult ViewPdf(string fileName)
        {
            var document = db.Documents.Find(fileName);
            return File(document.Data, "application/pdf");
        }

        //Deletes a document given its name. It redirects to Palette to show that the document is not available to view anymore.
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

        //Given the content(the htmlstring), the docName, a proto document with some relevant information, and the description, ensures that the document is created and successfully put into the database.
        [HttpPost]
        public IActionResult SaveNewDocument(string content, string DocumentName, Group9_iCareApp.Models.Document protoDoc, string Description)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(DocumentName) || db.Documents.Find(DocumentName + ".pdf") != null) 
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

            var treatment = db.TreatmentRecords.FirstOrDefault(p => p.TreatmentId == protoDoc.treatmentID);

            int workerID = FindCurrentWorkerId();
            Group9_iCareApp.Models.Document document = new()
            {
                DocumentName = DocumentName + ".pdf",
                Data = bytes,
                CreationDate = DateTime.Now,
                CreatingWorkerId = workerID,
                PatientRecordId = treatment.PatientId,
                LastModifiedDate = DateTime.Now,
                ModifyingWorkerId = workerID,
                Description = Description,
                treatmentID = treatment.TreatmentId
            };


            db.Documents.Add(document);
            db.SaveChanges();

            return RedirectToAction("Palette"); //go back to Viewing the list of docs to see update.
        }

        //Given the new content, the docName, and the new(possibly) description, updates the document in the database.
        [HttpPost]
        public IActionResult SaveOldDocument(string content, string docName, string Description)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(docName)) 
            {
                //nothing in doc or nothing in docName.
                return NoContent();
            }

            // Convert HTML content to PDF
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
                document.ModifyingWorkerId = FindCurrentWorkerId();
                document.Description = Description;
                db.SaveChanges();
            }
            return RedirectToAction("Palette");
        }

        //When uploading a document, ensures the proper treatments can be selected.
        public async Task<IActionResult> UploadDocument()
        {
            ViewData["treatments"] = await CreateTreatmentSelectList();
            return View();
        }

        //Given a file uploaded, a new description, and a protoDoc with relevant information, creates a new document to be put in the repository.
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, Group9_iCareApp.Models.Document protoDoc, string Description)
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

            int workerID = FindCurrentWorkerId();
            var treatment = db.TreatmentRecords.FirstOrDefault(p => p.TreatmentId == protoDoc.treatmentID);
            Group9_iCareApp.Models.Document document = new()
            {
                DocumentName = fileNameNoExtension + ".pdf",
                Data = pdfBytes,
                CreationDate = DateTime.Now,
                CreatingWorkerId = workerID,
                LastModifiedDate = DateTime.Now,
                ModifyingWorkerId = workerID,
                PatientRecordId = treatment.PatientId,
                Description = Description,
                treatmentID = treatment.TreatmentId,
            };

            db.Documents.Add(document);
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Document uploaded successfully!";

            return RedirectToAction("UploadDocument");
        }

        //converts pdf byte array to an htmlString to be displayed to edit.
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

        //converts a given Image to a PDF to store in the database or outputted.
        private static byte[] ConvertImageToPdf(Stream imageStream)
        {
            Aspose.Words.Document doc = new();
            var builder = new DocumentBuilder(doc);
            builder.InsertImage(imageStream); //insert image//

            using var outStream = new MemoryStream();
            doc.Save(outStream, SaveFormat.Pdf); //turn into PDF 
            return outStream.ToArray(); 
        }

        //converts a word doc to a PDF to be stored in the database or outputted.
        private static byte[] ConvertWordToPdf(Stream wordStream)
        {
            Aspose.Words.Document doc = new(wordStream);

            using var outStream = new MemoryStream();
            doc.Save(outStream, SaveFormat.Pdf); //turn into PDF
            return outStream.ToArray();
        }

        //converts the PDF to bytes to be stored in the database
        private static byte[] ConvertPdfToBytes(Stream pdfStream)
        {
            Aspose.Pdf.Document pdf = new(pdfStream);

            using var outStream = new MemoryStream();
            pdf.Save(outStream);
            return outStream.ToArray();

        }

        //using the UserManager, finds the workerID associated with the logged in user.
        private int FindCurrentWorkerId()
        {
            return db.iCAREWorkers.FirstOrDefault(w => w.UserAccount == userManager.GetUserId(User)).Id;
        }
        //finds the assigned Patients in the database based on the logged in user.
        private async Task<List<int?>> FindAssignedPatientIds()
        {
            return await db.TreatmentRecords.Where(t => t.WorkerId == FindCurrentWorkerId()).Select(t => t.PatientId).Distinct().ToListAsync();
        }
        //Creates a Select list consisting of the patient's id and their fullName based on the assigned patients of the logged in user.
        private async Task<SelectList> CreatePatientSelectList()
        {
            var patientIDs = await FindAssignedPatientIds();
            var patients = await db.PatientRecords.Where(p => patientIDs.Contains(p.Id)).Select(p => new {
                Id = p.Id,
                fullName = p.Fname + " " + p.Lname
            }).ToListAsync();
            return new SelectList(patients, "Id", "fullName");
        }

        //Creates a Select List consisting of the treatment's id and the description based on the treatments done by the user.
        private async Task<SelectList> CreateTreatmentSelectList()
        {
            var patientIDs = await FindAssignedPatientIds();
            var treatments = await db.TreatmentRecords.Where(t => patientIDs.Contains(t.PatientId)).Select(t => new
            {
                Id = t.TreatmentId,
                desc = t.Description,
            }).ToListAsync();
            return new SelectList(treatments, "Id", "desc");
        }

    }  
}
