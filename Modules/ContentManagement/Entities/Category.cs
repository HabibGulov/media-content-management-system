public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public IEnumerable<ContentCategory> ContentCategories { get; set; } = [];
}