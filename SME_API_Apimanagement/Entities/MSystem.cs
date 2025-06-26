using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class MSystem
{
    public int Id { get; set; }

    public string? SystemCode { get; set; }

    public string? SystemName { get; set; }

    public bool? FlagActive { get; set; }

    public string? FlagDelete { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? Runing { get; set; }

    public string? Note { get; set; }

    public string? OwnerSystemCode { get; set; }
}
