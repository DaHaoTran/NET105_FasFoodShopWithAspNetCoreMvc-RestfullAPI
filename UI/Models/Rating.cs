using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RateId { get; set; }

        public int Value { get; set; }

        public int? OrderId { get; set; }

        [Column(TypeName = "Char(5)")]
        public string? FoodCode { get; set; }

        [ForeignKey("FoodCode")]
        public virtual Food? Food { get; set; }

        [Column(TypeName = "Varchar(200)")]
        public string? CustomerEmail { get; set; }

        [ForeignKey("CustomerEmail")]
        public virtual Customer? Customer { get; set; }
    }
}
