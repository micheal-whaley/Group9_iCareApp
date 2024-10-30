using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group9_iCareApp.Models;

public partial class iCAREUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [Required]
    public string? Password { get; set; }

    //public virtual iCAREAdmin? iCAREAdmin { get; set; }

    //public virtual iCAREWorker? iCAREWorker { get; set; }

    //public virtual UserPassword? UserPassword { get; set; }
}
