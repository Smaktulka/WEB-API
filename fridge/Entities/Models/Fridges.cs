using Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace fridge.Models
{
    public class Fridges
    {
        [Column("FridgeId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Fridge Name is required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length of Name is 60 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Fridge Owner_Name is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length of Owner_Name is 30 characters.")]
        public string? Owner_Name { get; set; }

        [ForeignKey(nameof(FridgeModel))]
        public Guid ModelId { get; set; }
        public FridgeModels FridgeModel { get; set; } = null!;
    }
}
