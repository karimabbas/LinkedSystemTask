using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LinkSystem.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? HashPassword {get;set;}
        public string? SaltPassword {get;set;}

    }
}