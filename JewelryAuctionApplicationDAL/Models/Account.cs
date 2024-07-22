

namespace JewelryAuctionApplicationDAL.Models;

public enum Role
{
    ADMIN,
    MANAGER,
    STAFF,
    USER
}
public class Account
{
    public int AccountId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool Status { get; set; }
    public decimal Credit { get; set; }
    public Role Role { get; set; }
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public ICollection<Auction> Auctions { get; set; } = new List<Auction>();
}
