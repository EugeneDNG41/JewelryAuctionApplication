

namespace JewelryAuctionApplicationDAL.Models;
//[Table("Bid")]
public class Bid
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BidId { get; set; }
    //[Required]
    public decimal BidAmount { get; set; }
    //[Required]
    public DateTime BidTime { get; set; }
   // [Required]
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    //[Required]
    //[ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
