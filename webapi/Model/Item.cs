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

    // Changes in status
    public void Claim(string claimedBy)
    {
        if (Status != ItemStatus.Available)
        {
            throw new InvalidOperationException(
                "Only available items can be claimed");
        }

        Status = ItemStatus.Claimed;
        ClaimedBy = claimedBy;
        ClaimedAtUtc = DateTime.UtcNow;
    }

    public void Return()
    {
        if (Status != ItemStatus.Claimed)
        {
            throw new InvalidOperationException(
                "Only claimed items can be returned");
        }

        Status = ItemStatus.Returned;
        ReturnedAtUtc = DateTime.UtcNow;
    }
    
    public void EnsureCanDelete()
    {
        if (Status != ItemStatus.Available)
        {
            throw new InvalidOperationException(
                "Only available items can be deleted");
        }
    }
}