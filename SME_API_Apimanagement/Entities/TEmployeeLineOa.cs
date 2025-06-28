using System;
using System.Collections.Generic;

namespace SME_API_Apimanagement.Entities;

public partial class TEmployeeLineOa
{
    public int Id { get; set; }

    public string? EmployeeId { get; set; }

    public string? LineOaUserId { get; set; }

    public string? LineOaAccessToken { get; set; }

    public string? LineOaRefreshToken { get; set; }

    public string? LineOaDisplayName { get; set; }

    public string? LineOaPictureUrl { get; set; }

    public DateTime? LineOaDateJoined { get; set; }
}
