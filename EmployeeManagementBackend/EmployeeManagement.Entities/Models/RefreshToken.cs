using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
