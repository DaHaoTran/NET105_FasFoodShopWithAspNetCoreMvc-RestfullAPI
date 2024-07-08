using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class FoodType
    {
        [Key]
        [Column(TypeName = "Char(4)")]
        public string FTypeCode {  get; set; }

        [MaxLength(200)]
        public string TypeName { get; set; }

        public ICollection<Food> foods { get; set; }
    }
}
