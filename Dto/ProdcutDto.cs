using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSystem.Dto
{
    public class ProdcutDto
    {
        public string? Name { get; set; }
        public string? Descripition { get; set; }
        public double? Price { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}