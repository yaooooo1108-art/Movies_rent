using System.ComponentModel.DataAnnotations;

namespace _1131754.Models
{

    public class Order
    {

        [Key]

        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<_1131754.Models.OrderDetail> OrderDetails { get; set; }
    }

}
