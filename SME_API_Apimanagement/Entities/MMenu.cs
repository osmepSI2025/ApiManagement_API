using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class MMenu
{
    public int MenuId { get; set; }

    public string? MenuName { get; set; }

    public string? MenuUrl { get; set; }

    public int? ParentId { get; set; }

    public string? Style { get; set; }

    public int? Ranking { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdateBy { get; set; }

    public string? FlagActive { get; set; }

    public string? FlagDelete { get; set; }
}
