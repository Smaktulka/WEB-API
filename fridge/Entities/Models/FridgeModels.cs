using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FridgeModels
    {
        [Column("FridgeModelId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "FridgeModel Name is required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Year is required field")]
        [Range(2000, 2022, ErrorMessage = "Year is required and it can't be lower than 2000 and higher than 2020")]
        public int Year { get; set; } = 0!;
    }

}
