using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group9_iCareApp.Models;

public partial class iCAREUser : IdentityUser
{
    //public int Id { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }



    public int locationID { get; set; }

    //public virtual iCAREAdmin? ICareadmin { get; set; }

    //public virtual iCAREWorker? ICareworker { get; set; }

   // public virtual Location Location { get; set; } = null!;

}
