using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class WorkerRole
{
    public string Profession { get; set; } = null!; //either doctor or nurse//

    public virtual ICollection<iCAREWorker> ICareworkers { get; set; } = new List<iCAREWorker>();
}
