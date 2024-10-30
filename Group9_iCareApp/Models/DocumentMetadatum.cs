using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class DocumentMetadatum
{
    public int DocId { get; set; }

    public string? DocName { get; set; }

    public DateTime? DateOfCreation { get; set; }

    public virtual ICollection<ModificationHistory> ModificationHistories { get; set; } = new List<ModificationHistory>();
}
