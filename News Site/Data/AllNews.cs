using System;
using System.Collections.Generic;

namespace News_Site.Data;

public partial class AllNews
{
    public int Id { get; set; }

    public string? SId { get; set; }

    public string? Title { get; set; }

    public string? Url { get; set; }

    public string? Category { get; set; }

    public string? MainPhoto { get; set; }

    public string? Content { get; set; }

    public string? Video { get; set; }

    public int? Coments { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorPhoto { get; set; }
}
