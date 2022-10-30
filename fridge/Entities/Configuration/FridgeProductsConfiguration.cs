using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class FridgeProductsConfiguration : IEntityTypeConfiguration<FridgeProducts>
    {
        public void Configure(EntityTypeBuilder<FridgeProducts> builder)
        {
            builder.HasData
                (
                    new FridgeProducts
                    {
                        Id = new Guid("598b7ca5-3f08-4bf0-b549-bc582003907f"),
                        ProductId = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd2"),
                        FridgeId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9994870"),
                        Quantity = 3,
                    },
                    new FridgeProducts
                    {
                        Id = new Guid("598b7ca5-3f08-4bf0-b549-bc5820039075"),
                        ProductId = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd3"),
                        FridgeId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991470"),
                        Quantity = 6,
                    },
                       new FridgeProducts
                       {
                           Id = new Guid("598b7ca5-3f08-4bf0-b549-bc5820039076"),
                           ProductId = new Guid("16ad116a-8289-4ce9-aa96-4d367dec6dd4"),
                           FridgeId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991470"),
                           Quantity = 7
                       }
                );

        }

    }
}
