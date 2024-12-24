using System;
using System.Collections.Generic;

namespace BingWallPaper.Server.Database;

public partial class Wallpaper
{
    public string Id { get; set; } = null!;

    public string? _4k { get; set; }

    public string? Desc { get; set; }

    public string? Author { get; set; }

    public DateOnly? Date { get; set; }
}
