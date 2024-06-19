using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplication.Models;

public enum JewelryCategory
{
    RINGS,
    BRACELETS,
    BROOCHES_PINS,
    CUFFLINKS_TIEPINS_TIECLIPS,
    EARRINGS,
    LOOSESTONES_BEADS,
    NECKLACES_PENDANTS,
    WATCHES
}
//[Table("Jewelry")]
public class Jewelry
{
    //[Key]
    public int JewelryId { get; set; }
    //[Required]
    public string JewelryName { get; set; }
    public string Description { get; set; }
    //[Required]
    public JewelryCategory JewelryCategory {  get; set; }
    //[Required]
    public string Condition { get; set; }
    //[Required]
    public string Estimate {  get; set; }
    //[Required]
    public decimal StartingPrice { get; set; }
    //[Required]
    public bool Status { get; set; }
    public ICollection<Auction> Auctions { get; set; }
    public ICollection<Request> Requests { get; set; }
}
