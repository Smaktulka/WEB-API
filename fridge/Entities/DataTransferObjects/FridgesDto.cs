using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class FridgesDto
    {
        [Column("FridgeId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Fridge Name is required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length of Name is 60 characters.")]
        public string Name { get; set; } = null!;

        public Guid ModelId { get; set; }

        [Required(ErrorMessage = "Fridge Owner_Name is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length of Owner_Name is 30 characters.")]
        public string? Owner_Name { get; set; }        
    }
}
