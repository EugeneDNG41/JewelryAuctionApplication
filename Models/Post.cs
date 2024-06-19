using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplication.Models;

public enum PostCategory
{
    RESOURCES,
    TIPS,
    TRENDS
}
//[Table("Post")]
public class Post
{
    //[Key]
    public int PostId { get; set; }
    //[Required]
    public string Title { get; set; }
    //[Required]
    public PostCategory PostCategory { get; set; }
    //[Required]
    public DateTime PostDate { get; set; }
    public string Body { get; set; }
    //[Required]
    public bool Status { get; set; }
    public string Image { get; set; }
    //[Required]
    //[ForeignKey(nameof(Account))]
    public int AccountId { get; set; }
    public Account Author { get; set; }
   
}
