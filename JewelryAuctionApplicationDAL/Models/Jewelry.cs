

namespace JewelryAuctionApplicationDAL.Models;

public enum JewelryCategory
{
    RINGS,
    BRACELETS,
    BROOCHES_PINS,
    CUFFLINKS_TIEPINS_TIECLIPS,
    EARRINGS,
    LOOSESTONES_BEADS,
    NECKLACES_PENDANTS,
    WATCHES
}
public enum JewelryStatus {
    VALUATING,
    READY,
    ACTIVE,
    SOLD,
    DELETED
}
public class Jewelry
{
    public int JewelryId { get; set; }
    public string JewelryName { get; set; }
    public string Description { get; set; }
    public JewelryCategory JewelryCategory {  get; set; }
    public string Condition { get; set; }
    public decimal StartingPrice { get; set; }
    public JewelryStatus Status { get; set; }
    public byte[] Image { get; set; }
    public ICollection<Auction> Auctions { get; set; }
}
