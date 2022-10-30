using fridge.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FridgeProducts
    {
        [Column("FridgeProductsId")]
        public Guid Id { get; set; }

        [ForeignKey(nameof (Products))]
        public Guid ProductId { get; set; }
        public Products Product { get; set; } = null!;

        [ForeignKey(nameof(Fridges))]
        public Guid FridgeId { get; set; } 
        public Fridges Fridge { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity is required and it can't be less than 0.")]
        public int Quantity { get; set; }
    }
}
