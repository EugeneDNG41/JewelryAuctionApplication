

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
//[Table("Jewelry")]
public class Jewelry
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JewelryId { get; set; }
    //[Required]
    public string JewelryName { get; set; }
    public string Description { get; set; }
    //[Required]
    public JewelryCategory JewelryCategory {  get; set; }
    //[Required]
    public string Condition { get; set; }
    //[Required]
    //public string Estimate {  get; set; }
    //[Required]
    public decimal StartingPrice { get; set; }
    //[Required]
    public bool Status { get; set; }
    public byte[] Image { get; set; }
    public ICollection<Auction> Auctions { get; set; }
    public ICollection<Request> Requests { get; set; }
}
