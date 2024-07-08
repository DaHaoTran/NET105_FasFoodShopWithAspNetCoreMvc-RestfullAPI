using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class FoodCategory
    {
        [Key]
        [Column(TypeName = "Char(4)")]
        public string? FCategoryCode { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }

        [Column(TypeName = "Varchar(Max)")]
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        public ICollection<Food>? foods { get; set; }
    }
}
