using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        public int UnitPrice { get; set; }

        public int Quantity { get; set; }

        public bool? Rated { get; set; } = false;

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [Column(TypeName = "Char(5)")]
        public string FoodCode { get; set; }

        [ForeignKey("FoodCode")]
        public virtual Food? Food { get; set; }
    }
}
