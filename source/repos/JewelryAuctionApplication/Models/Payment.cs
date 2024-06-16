using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Models;

public enum Status
{
    PENDING,
    COMPLETED,
    OVERDUE,
    FAILED,
    REFUNDED
}
//[Table("Payment")]
public class Payment
{
    //[Key]
    public int PaymentId { get; set; }
    //[Required]
    public string PaymentMethod { get; set; }
    //[Required]
    public decimal Subtotal { get; set; }
    //[Required]
    public decimal Tax { get; set; }
    //[Required]
    public decimal Shipping { get; set; }
    //[Required]
    public decimal Total { get; set; }
    //[Required]
    public Status Status { get; set; }
    //[Required]
    //[ForeignKey(nameof(Auction))]
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    //[Required]
    //[ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
