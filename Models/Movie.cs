using System.ComponentModel.DataAnnotations;
using Humanizer;

namespace _1131754.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        public string Name { get; set; }
     
        public string Type { get; set; }
       
        public decimal Price { get; set; }
      
        public int Quantity { get; set; }
    
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
