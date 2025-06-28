using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class MLineOa
{
    public int Id { get; set; }

    public string? LineOaName { get; set; }

    public string? LineOaChannelId { get; set; }

    public string? LineOaChannelSecret { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
