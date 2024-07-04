

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
    //[Required]
    //public DateOnly Birthday { get; set; }
    //[RegularExpression(@"^[0-9]{8,12}$", ErrorMessage = "Telephone must include 8-12 digits")]
    //public string PhoneNumber { get; set; }
    //[Required]
    public bool Status { get; set; }
    //[Required]

    public Role Role { get; set; }
    public ICollection<Bid> Bids { get; set; }
    public ICollection<Request> Requests { get; set; }
    public ICollection<Payment> Payments { get; set; }
    
}
