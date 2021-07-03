using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Models
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [Display(Name = "Email Adresi")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
