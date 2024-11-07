using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class Document
{
    public int Id { get; set; }

    public byte[] Data { get; set; } = null!;

    public int? PatientRecordId { get; set; }

    public DateTime CreationDate { get; set; }

    public int? CreatingWorkerId { get; set; }

    public string DocumentName { get; set; } = null!;

    public DateTime LastModifiedDate { get; set; }

    public int? ModifyingWorkerId { get; set; }

    public string? Description { get; set; }

    //public virtual iCAREWorker? CreatingWorker { get; set; }

    //public virtual iCAREWorker? ModifiyingWorker { get; set; }

    //public virtual PatientRecord? Patient { get; set; }
}
