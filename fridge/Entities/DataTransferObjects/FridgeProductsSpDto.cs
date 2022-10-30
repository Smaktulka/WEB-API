using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class FridgeProductsSpDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid FridgeId { get; set; }
        public int Quantity { get; set; }
    }
}
