using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class MRole
{
    public int Id { get; set; }

    public string? RoleCode { get; set; }

    public string? RoleName { get; set; }

    public bool? FlagActive { get; set; }

    public string? FlagDelete { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
