using System;
using System.Collections.Generic;

namespace News_Site.Data;

public partial class Article
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Url { get; set; }

    public string? Category { get; set; }

    public string? MainPhoto { get; set; }

    public string? Content { get; set; }

    public string? Keywords { get; set; }

    public DateTime? PostedAt { get; set; }

    public int? Rank { get; set; }

    public string? Website { get; set; }
}
