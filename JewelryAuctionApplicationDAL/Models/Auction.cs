
namespace JewelryAuctionApplicationDAL.Models;
//[Table("Auction")]
public class Auction
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AuctionId { get; set; }
    //[Required]
    public DateTime EndDate { get; set; }
    //[Required]
    //[Range(0, double.MaxValue)]
    public decimal CurrentPrice { get; set; }
    //[Required]
    //[ForeignKey(nameof(Jewelry))]
    public int JewelryId { get; set; }
    public Jewelry Jewelry { get; set; }
    public ICollection<Bid> Bids { get; set; }
}
