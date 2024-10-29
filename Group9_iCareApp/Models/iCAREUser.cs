using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class iCAREUser
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual iCAREAdmin? iCAREAdmin { get; set; }

    public virtual iCAREWorker? iCAREWorker { get; set; }

    public virtual UserPassword? UserPassword { get; set; }
}
