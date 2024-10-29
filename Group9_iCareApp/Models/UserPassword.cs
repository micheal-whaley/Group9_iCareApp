using System;
using System.Collections.Generic;

namespace Group9_iCareApp.Models;

public partial class UserPassword
{
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? PasswordExpiryTime { get; set; }

    public DateOnly? UserAccountExpiryDate { get; set; }

    public virtual iCAREUser IdNavigation { get; set; } = null!;
}
