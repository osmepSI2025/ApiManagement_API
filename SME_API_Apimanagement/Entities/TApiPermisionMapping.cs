using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class TApiPermisionMapping
{
    public int Id { get; set; }

    public string? OrganizationCode { get; set; }

    public string? SystemCode { get; set; }

    public string? ApiKey { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? FlagActive { get; set; }

    public string? FlagDelete { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? SystemApiMappingId { get; set; }
}
