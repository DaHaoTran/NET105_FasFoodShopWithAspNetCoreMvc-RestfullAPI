using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class Food
    {
        [Key]
        [Column(TypeName = "Char(5)")]
        public string? FoodCode { get; set; }

        [MaxLength(300)]
        [Required(ErrorMessage = "Food name is required")]
        public string FoodName { get; set; }

        [Required(ErrorMessage = "Current Price is required")]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "Current Price does't have characters")]
        public int CurrentPrice { get; set; }

        public int? PreviousPrice { get; set; }

        [Required(ErrorMessage = "Left is required")]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "Left does't have characters")]
        public int Left { get; set; }

        [RegularExpression(@"^-?\d+$", ErrorMessage = "Sold does't have characters")]
        public int? Sold { get; set; }

        [Column(TypeName = "Varchar(Max)")]
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Column(TypeName = "Char(4)")]
        [Required(ErrorMessage = "Food type code is required")]
        public string FTypeCode { get; set; }

        [ForeignKey("FTypeCode")]
        public virtual FoodType? FoodType { get; set; }

        [Column(TypeName = "Char(4)")]
        [Required(ErrorMessage = "Food category code is required")]
        public string FCategoryCode { get; set; }

        [ForeignKey("FCategoryCode")]
        public virtual FoodCategory? FoodCategory { get; set; }


        [Column(TypeName = "Char(5)")]
        public string? AdminCode { get; set; }

        [ForeignKey("AdminCode")]
        public virtual Admin? Admin { get; set; }

        public ICollection<Rating>? Ratings { get; set; }

        public ICollection<CartItem>? CartItems { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        [NotMapped]
        public int? Quantity { get; set; }

        [NotMapped]
        public bool? Rated { get; set; }

        [NotMapped]
        public int? OrderId { get; set; }
    }
}
