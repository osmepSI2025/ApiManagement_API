using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class TRolePermission
{
    public int Id { get; set; }

    public int? MenuId { get; set; }

    public string? RoleCode { get; set; }

    public string? PermissionCode { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
