using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class PatientRecord
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public string? BloodGroup { get; set; }

    public string? BedId { get; set; }

    public string? TreatmentArea { get; set; }

    public virtual ICollection<TreatmentRecord> TreatmentRecords { get; set; } = new List<TreatmentRecord>();
}
