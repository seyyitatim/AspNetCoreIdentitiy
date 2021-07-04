using Asp.NetCoreIdentity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Models
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "{0} alanı gereklidir")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "{0} alanı gereklidir")]
        [Display(Name = "Tel no")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "{0} alanı gereklidir")]
        [Display(Name = "E-Posta")]
        [EmailAddress(ErrorMessage = "{0} adresini doğru formatta giriniz")]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDay { get; set; }
        [Display(Name = "Resim")]
        public IFormFile Image { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
    }
}
