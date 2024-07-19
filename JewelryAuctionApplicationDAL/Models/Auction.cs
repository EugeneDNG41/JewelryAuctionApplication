
namespace JewelryAuctionApplicationDAL.Models;
public class Auction
{
    public int AuctionId { get; set; }
    public DateTime EndDate { get; set; }
    public decimal CurrentPrice { get; set; }
    public int JewelryId { get; set; }
    public Jewelry Jewelry { get; set; }
    public int? AccountId { get; set; }
    public Account? Account { get; set; }
    public ICollection<Bid> Bids { get; set; }
}
