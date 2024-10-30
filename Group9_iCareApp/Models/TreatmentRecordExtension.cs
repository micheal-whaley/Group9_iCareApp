using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group9_iCareApp.Models
{
    public partial class TreatmentRecordService
    {
        private readonly iCAREDBContext _context;

        public TreatmentRecordService(iCAREDBContext context)
        {
            _context = context;
        }

        public void SetRecord(int workerID, List<int> selectedPatients)
        {
            // Get worker with id from database.
            var worker = _context.iCAREWorkers.FirstOrDefault(w => w.Id == workerID);
            if (worker == null)
            {
                throw new KeyNotFoundException($"Worker with ID {workerID} not found.");
            }

            // Iterate through each selected patient ID and create a new treatment record.
            foreach (var patientID in selectedPatients)
            {
                var patient = _context.PatientRecords.FirstOrDefault(p => p.Id == patientID);
                if (patient == null)
                {
                    throw new KeyNotFoundException($"Patient with ID {patientID} not found.");
                }

                // Create new treatment record for patient.
                var newTreatmentRecord = new TreatmentRecord
                {
                    TreatmentId = Guid.NewGuid().ToString(), // Generate new unique treatment ID
                    WorkerId = workerID,
                    PatientId = patientID,
                    Description = $"Assigned to worker {workerID}",
                    TreatmentDate = DateTime.UtcNow,
                    Worker = worker,
                    Patient = patient
                };

                // Add treatment record to context
                _context.TreatmentRecords.Add(newTreatmentRecord);
            }

            _context.SaveChanges(); // Save changes
        }

        public List<int?> GetPatientsID(int workerID) // can return null.
        {
            var treatmentRecords = _context.TreatmentRecords
                .Where(tr => tr.WorkerId == workerID)
                .Select(tr => tr.PatientId)
                .ToList();

            return treatmentRecords;
        }
    }

}