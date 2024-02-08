using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSystem.Dto
{
    public class UserRegister
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}