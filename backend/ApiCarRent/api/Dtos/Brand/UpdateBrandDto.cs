using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Brand
{
    public class UpdateBrandDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "BrandName must be 3 characters")]
        [MaxLength(100, ErrorMessage = "BrandName cannot be over 100 characters")]
        public string BrandName { get; set; } = string.Empty;
    }
}