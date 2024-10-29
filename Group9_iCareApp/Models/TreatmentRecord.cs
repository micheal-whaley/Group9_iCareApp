using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class TreatmentRecord
{
    public string TreatmentId { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? TreatmentDate { get; set; }

    public string? PatientId { get; set; }

    public string? WorkerId { get; set; }

    public virtual PatientRecord? Patient { get; set; }

    public virtual iCAREWorker? Worker { get; set; }
}
