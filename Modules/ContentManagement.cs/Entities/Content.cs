public class Content : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsFree { get; set; }
    public double? Price { get; set; }
    public Guid UserId { get; set; }
    public ContentType ContentType { get; set; }

    public User Author { get; set; } = default!;
    public IEnumerable<ContentCategory> ContentCategories{get; set;} =[];
}
