using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

namespace UI.Models
{
    public class Admin
    {
        [Key]
        [Column(TypeName = "Char(5)")]
        public string? AdminCode { get; set; }

        [Column(TypeName = "Varchar(200)")]
        [EmailAddress(ErrorMessage = "Email must be in correct format")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Column(TypeName = "Varchar(100)")]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{5,}$", ErrorMessage = "Password has 5 characters or more include less 1 number and less 1 uppercase letters")]
        public string Password { get; set; }

        public bool? IsOnl { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? Level {  get; set; }

        public ICollection<Customer>? customers { get; set; }

        public ICollection<Food>? foods { get; set; }
    }
}
