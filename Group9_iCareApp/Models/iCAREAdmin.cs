using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class iCAREAdmin
{
    public int Id { get; set; }

    public virtual iCAREUser IdNavigation { get; set; } = null!;
}
