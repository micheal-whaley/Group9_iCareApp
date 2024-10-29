using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class iCAREAdmin
{
    public string Id { get; set; } = null!;

    public string? AdminEmail { get; set; }

    public DateOnly? DateHired { get; set; }

    public DateOnly? DateFinished { get; set; }

    public virtual iCAREUser IdNavigation { get; set; } = null!;
}
