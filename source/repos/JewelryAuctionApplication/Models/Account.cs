using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Models;

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
    public int AccountId { get; set; }
    //[Required]
    public string Username { get; set; }
    //[Required]
    //[Range(8,20)]
    public string Password { get; set; }
    //[Required]
    //[MaxLength(100)]
    public string FullName { get; set; }
    //[Required]
    //[EmailAddress]
    public string Email { get; set; }
    public string Image {  get; set; }
    //[Required]
    public DateTime Birthday { get; set; }
    //[Required]
    //[RegularExpression(@"^[0-9]{8,12}$", ErrorMessage = "Telephone must include 8-11 digits")]
    public string PhoneNumber { get; set; }
    //[Required]
    public bool Status { get; set; }
    //[Required]

    public Role Role { get; set; }
    public ICollection<Bid> Bids { get; set; }
    public ICollection<Request> Requests { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Post> Posts { get; set; }
    
}
