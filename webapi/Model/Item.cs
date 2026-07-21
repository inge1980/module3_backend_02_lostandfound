namespace webapi.Model;

public class Item
{
    public Guid Id { get; private set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string FoundLocation { get; set; } = string.Empty;

    public DateTime FoundAtUtc { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.Available;

    public string ClaimedBy { get; set; } = string.Empty;

    public DateTime? ClaimedAtUtc { get; set; }

    public DateTime? ReturnedAtUtc { get; set; }
    
    // Initializes a new instance with default values.
    public Item()
    {
        Id = Guid.NewGuid();
        //FoundAtUtc = DateTime.UtcNow;
        //Status = ItemStatus.Available;
    }
}