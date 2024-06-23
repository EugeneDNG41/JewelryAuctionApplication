using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Models;
public enum RequestStatus
{
    PENDING,
    PRELIMINARY_PROCESSING,
    AWAIT_RESPONSE,
    ARRIVED,
    FINAL_PROCESSING,
    COMPLETED,
    CANCELED
}
//[Table("Request")]
public class Request
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RequestId { get; set; }
    //[Required]
    public DateTime RequestDate { get; set; }
    //[Required]
    public DateTime ValuationDate { get; set; }
    public decimal PreliminaryValuation { get; set; }
    public decimal FinalValuation { get; set; }
    //[Required]
    public RequestStatus Status { get; set; }
    //[Required]
    //[ForeignKey(nameof(Jewelry))]
    public int JewelryId { get; set; }
    public Jewelry Jewelry { get; set; }
    //[Required]
    //[ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
