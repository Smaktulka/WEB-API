using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ProductsDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Products Name is required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length of Name is 60 characters.")]
        public string? Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Default_Quantity is required and can't be less than 1.")]
        public int Default_Quantity { get; set; } = 0!;
    }
}
