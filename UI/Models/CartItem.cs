using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }

        [Column(TypeName = "Char(5)")]
        public string FoodCode { get; set; }

        [ForeignKey("FoodCode")]
        public virtual Food? Food { get; set; }

    }
}
