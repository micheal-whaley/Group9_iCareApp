using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class DrugsDictionary
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
