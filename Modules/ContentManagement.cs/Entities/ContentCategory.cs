public class ContentCategory : BaseEntity
{
    public Guid CategoryId { get; set; }
    public Guid ContentId { get; set; }

    public Category Category { get; set; } = default!;
    public Content Content { get; set; } = default!;
}