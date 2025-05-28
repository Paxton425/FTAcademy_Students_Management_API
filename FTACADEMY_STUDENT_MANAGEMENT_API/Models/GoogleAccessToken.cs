using System;
using System.Collections.Generic;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

public partial class GoogleAccessToken
{
    public int TokenId { get; set; }

    public int? UserId { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public string? AccessTokenProvider { get; set; }

    public DateTime LastUpdate { get; set; }

    public DateTime TokenExpiry { get; set; }

    public virtual User? User { get; set; }
}
