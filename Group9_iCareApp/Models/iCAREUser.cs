using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group9_iCareApp.Models;

public partial class iCAREUser
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Password { get; set; } = null!;

    //public virtual iCAREAdmin? iCAREAdmin { get; set; }

    //public virtual iCAREWorker? iCAREWorker { get; set; }
}
