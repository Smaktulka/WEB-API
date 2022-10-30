using fridge.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Entities.Configuration
{
    public class FridgeModelConfiguration : IEntityTypeConfiguration<FridgeModels>
    {
        public void Configure(EntityTypeBuilder<FridgeModels> builder)
        {
            builder.HasData
                (
                    new FridgeModels
                    {
                        Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                        Name = "LG",
                        Year = 12,
                    },
                    new FridgeModels
                    {
                        Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991879"),
                        Name = "JH",
                        Year = 10,
                    }
                );

        }
    }
}
