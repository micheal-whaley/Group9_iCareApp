using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class PatientRecord
{
    public int Id { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public string? BloodGroup { get; set; }

    public string? BedId { get; set; }

    public string? TreatmentArea { get; set; }

    public int LocationId { get; set; }
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<TreatmentRecord> TreatmentRecords { get; set; } = new List<TreatmentRecord>();
}
