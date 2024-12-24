using System;
using System.Collections.Generic;

namespace BingWallPaper.Server.Database;

public partial class WallpaperCn
{
    public string Id { get; set; } = null!;

    public string? UrlBase { get; set; }

    public string? Desc { get; set; }

    public DateOnly? Date { get; set; }
}
