using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _1131754.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int MovieID { get; set; }
        [ForeignKey("MovieID")]     
        public Movie Movie { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public int UserID { get; set; } // 可選：若有登入系統
    }
}
