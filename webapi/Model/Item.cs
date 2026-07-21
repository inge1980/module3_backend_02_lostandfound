namespace webapi.Model;

/*
Id (Guid)
Title (kort tittel, f.eks. ?Svart lommebok?)
Description (valgfri)
Category (f.eks. ?Keys?, ?Wallet?, ?Clothing?, ?Other? ? kan være string eller enum)
FoundLocation (f.eks. ?Inngang A?)
FoundAtUtc (DateTime, UTC)
Status (enum)
ClaimedBy (valgfri string)
ClaimedAtUtc (valgfri DateTime)
ReturnedAtUtc (valgfri DateTime)
*/

public class Item
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string FoundLocation { get; set; } = string.Empty;

    public DateTime FoundAtUtc { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.Available;

    public string ClaimedBy { get; set; } = string.Empty;

    public DateTime? ClaimedAtUtc { get; set; }

    public DateTime? ReturnedAtUtc { get; set; }
    
}