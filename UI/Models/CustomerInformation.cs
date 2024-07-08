using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class CustomerInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CInforId { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage = "Tên người nhận không được bỏ trống")]
        public string CustomerName { get; set; }

        [Column(TypeName = "Char(10)")]
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [Phone(ErrorMessage = "Số điện thoại chưa đúng")]
        [MinLength(10, ErrorMessage = "Số điện thoại chưa đúng"), MaxLength(10, ErrorMessage = "Số điện thoại chưa đúng")]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        [Column(TypeName = "Varchar(200)")]
        public string? CustomerEmail { get; set; }

        [ForeignKey("CustomerEmail")]
        public virtual Customer? Customer { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Tên đường không được bỏ trống")]
        public string street { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Thành phố không được bỏ trống")]
        public string city { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Quận không được bỏ trống")]
        public string district { get; set; }
    }
}
