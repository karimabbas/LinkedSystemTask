using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSystem.Models
{
    public class Prodcut
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is Requierd")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is Requierd")]
        public string? Descripition { get; set; }
        public double? Price { get; set; }
        public string? Image { get; set; }
    }
}