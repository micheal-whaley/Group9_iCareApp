using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<iCAREUser> ICareusers { get; set; } = new List<iCAREUser>();

    public virtual ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();
}
