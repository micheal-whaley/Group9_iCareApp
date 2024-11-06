using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class iCAREWorker
{
    public int Id { get; set; }

    public string Profession { get; set; } = null!;

    public string UserAccount { get; set; } = null!;

    public virtual iCAREUser AccountNavigation { get; set; } = null!;

    public virtual ICollection<ModificationHistory> ModificationHistories { get; set; } = new List<ModificationHistory>();

    public virtual WorkerRole ProfessionNavigation { get; set; } = null!;


    public virtual ICollection<TreatmentRecord> TreatmentRecords { get; set; } = new List<TreatmentRecord>();
}
