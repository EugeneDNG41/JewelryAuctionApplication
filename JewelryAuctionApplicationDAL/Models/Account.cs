

namespace JewelryAuctionApplicationDAL.Models;

public enum Role
{
    ADMIN,
    MANAGER,
    STAFF,
    USER
}
//[Table("Account")]
public class Account
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccountId { get; set; }
    //[Required]
    public string Username { get; set; }
    //[Required]
    public string Password { get; set; }
    //[Required]
    //[MaxLength(100)]
    public string FullName { get; set; }
    //[Required]
    //[EmailAddress]
    public string Email { get; set; }
    public bool Status { get; set; }
    public decimal Credit { get; set; }
    //[Required]
    public Role Role { get; set; }
    public ICollection<Bid> Bids { get; set; }
    public ICollection<Request> Requests { get; set; }
    
}
