using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRMOZ.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} không được trống!")]
        [Display(Name = "Tên đăng nhập")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} không được trống!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Nhớ mật khẩu?")]
        public bool RememberMe { get; set; }
    }
}