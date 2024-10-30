using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class ModificationHistory
{
    public int ModificationId { get; set; }

    public DateTime? DateOfModification { get; set; }

    public string? Description { get; set; }

    public int? DocId { get; set; }

    public int? WorkerId { get; set; }

    public virtual DocumentMetadatum? Doc { get; set; }

    public virtual iCAREWorker? Worker { get; set; }
}
