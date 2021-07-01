using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage="{0} alanı gereklidir")]
        [Display(Name ="Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "{0} alanı gereklidir")]
        [Display(Name = "Tel no")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "{0} alanı gereklidir")]
        [Display(Name = "E-Posta")]
        [EmailAddress(ErrorMessage ="{0} adresini doğru formatta giriniz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} alanı gereklidir")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
