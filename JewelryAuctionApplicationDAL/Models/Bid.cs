

namespace JewelryAuctionApplicationDAL.Models;
public class Bid
{
    public int BidId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTime BidTime { get; set; }
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
