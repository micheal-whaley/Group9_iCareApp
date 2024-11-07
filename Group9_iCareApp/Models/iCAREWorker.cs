using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class iCAREWorker
{
    public int Id { get; set; }

    public string Profession { get; set; } = null!;

    public string ProfessionNavigationProfession { get; set; } = null!;

    public string? WorkerRoleProfession { get; set; }

    public string? AccountNavigationId { get; set; }

    public string UserAccount { get; set; } = null!;

    public virtual iCAREUser AccountNavigation { get; set; } = null!;

    public virtual WorkerRole ProfessionNavigation { get; set; } = null!;


    public virtual ICollection<TreatmentRecord> TreatmentRecords { get; set; } = new List<TreatmentRecord>();
}
