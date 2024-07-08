using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UI.Models
{
    public class Customer
    {
        [Key]
        [Column(TypeName = "Varchar(200)")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be in correct format")]
        public string Email { get; set; }

        [Column(TypeName = "Varchar(100)")]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{5,}$", ErrorMessage = "Password has 5 characters or more include less 1 number and less 1 uppercase letters")]
        public string PassWord { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirm password is required")]
        [Column(TypeName = "Varchar(100)")]
        [Compare(nameof(PassWord), ErrorMessage = "Confirm password is not same password")]
        public string ConfirmPassWord { get; set; }

        [Column(TypeName = "Varchar(300)")]
        public string? UserName { get; set; }

        [Column(TypeName = "Char(5)")]
        public string? AdminCode { get; set; }

        [ForeignKey("AdminCode")]
        public virtual Admin? Admins { get; set; }

        public ICollection<CustomerInformation>? CustomerInformations { get; set; }

        public virtual Cart? Cart { get; set; }

        public ICollection<Order>? orders { get; set; }

        public ICollection<Rating>? ratings { get; set; }    
    }
}
