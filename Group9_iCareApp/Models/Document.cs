using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class Document
{
    public int Id { get; set; }

    public byte[] Data { get; set; } = null!;

    public int? PatientId { get; set; }

    public DateOnly? CreationDate { get; set; }

    public int? WorkerId { get; set; }

    public string DocumentName { get; set; } = null!;

    //public virtual PatientRecord Id1 { get; set; } = null!;

    //public virtual ICareworker IdNavigation { get; set; } = null!;
}
