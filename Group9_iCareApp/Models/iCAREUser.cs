using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group9_iCareApp.Models;

public partial class iCAREUser
{
    public int Id { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int LocationId { get; set; }

    //public virtual iCAREAdmin? ICareadmin { get; set; }

    //public virtual iCAREWorker? ICareworker { get; set; }

    public virtual Location Location { get; set; } = null!;

}
