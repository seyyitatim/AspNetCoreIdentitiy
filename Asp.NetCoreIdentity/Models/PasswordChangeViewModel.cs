using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Models
{
    public class PasswordChangeViewModel
    {
        [Display(Name ="Eski Şifreniz")]
        [Required(ErrorMessage ="Eski şifreniz gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Şifreniz 4 ya da daha fazla karakterden oluşmalıdır")]
        public string PasswordOld { get; set; }
        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Yeni şifreniz gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz 4 ya da daha fazla karakterden oluşmalıdır")]
        public string PasswordNew { get; set; }
        [Display(Name = "Onay Yeni Şifreniz")]
        [Required(ErrorMessage = "Onay yeni şifreniz gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz 4 ya da daha fazla karakterden oluşmalıdır")]
        [Compare("PasswordNew",ErrorMessage ="Yeni şifreniz eşleşmiyor")]
        public string PasswordConfirm { get; set; }
    }
}
