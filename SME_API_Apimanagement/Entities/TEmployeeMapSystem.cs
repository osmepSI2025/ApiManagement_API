using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class TEmployeeMapSystem
{
    public int Id { get; set; }

    public int? SystemApiId { get; set; }

    public string? EmployeeId { get; set; }

    public bool? FlagActive { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
