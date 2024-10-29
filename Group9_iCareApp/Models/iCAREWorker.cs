using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class iCAREWorker
{
    public string Id { get; set; } = null!;

    public string? Profession { get; set; }

    public virtual iCAREUser IdNavigation { get; set; } = null!;

    public virtual ICollection<ModificationHistory> ModificationHistories { get; set; } = new List<ModificationHistory>();

    public virtual ICollection<TreatmentRecord> TreatmentRecords { get; set; } = new List<TreatmentRecord>();
}
