using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Models;
//[Table("Bid")]
public class Bid
{
    //[Key]
    public int BidId { get; set; }
    //[Required]
    public decimal BidAmount { get; set; }
    //[Required]
    public DateTime BidTime { get; set; }
    //[Required]
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    //[Required]
    //[ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
