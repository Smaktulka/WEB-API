using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasData
                (
                    new Products
                    {
                        Id = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd2"),
                        Name = "Sigi",
                        Default_Quantity = 3,
                    },
                    new Products
                    {
                        Id = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd3"),
                        Name = "Nigi",
                        Default_Quantity = 6,
                    },
                    new Products
                    {
                        Id = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd4"),
                        Name = "Nigi",
                        Default_Quantity = 6,
                    }
                ) ;
        }
    }
}
