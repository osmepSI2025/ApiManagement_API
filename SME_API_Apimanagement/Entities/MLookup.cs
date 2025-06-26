using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class MLookup
{
    public int Id { get; set; }

    public string? LookupType { get; set; }

    public string? LookupCode { get; set; }

    public string? LookupValue { get; set; }

    public string? LookupDescription { get; set; }

    public string? FlagActive { get; set; }

    public string? FlagDelete { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
