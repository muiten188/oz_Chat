using System.ComponentModel.DataAnnotations;

namespace CRMOZ.Web.Models
{
    public class RegisterViewModel
    {
        public string Id { set; get; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [Display(Name = "Họ Tên")]
        public string FullName { get; set; }
        
        [Display(Name = "Hình Đại Diện")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} ký tự!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập Lại Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và nhập lại mật khẩu không khớp!")]
        public string ConfirmPassword { get; set; }

        //public string Code { get; set; }
    }
}