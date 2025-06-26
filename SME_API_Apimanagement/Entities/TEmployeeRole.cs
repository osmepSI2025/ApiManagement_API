using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class TEmployeeRole
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string? RoleCode { get; set; }

    public string? BusinessUnitId { get; set; }

    public string? PositionId { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
