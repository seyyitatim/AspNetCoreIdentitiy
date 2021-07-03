using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Models
{
    public class NewPasswordViewModel
    {
        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmalıdır.")]
        public string Password { get; set; }
    }
}
