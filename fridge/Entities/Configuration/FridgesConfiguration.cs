using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using fridge.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class FridgesConfiguration : IEntityTypeConfiguration<Fridges>
    {
        public void Configure(EntityTypeBuilder<Fridges> builder)
        {
            builder.HasData
                (
                    new Fridges
                    {
                        Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9994870"),
                        Name = "LG",
                        Owner_Name = "Rakun",
                        ModelId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    },
                    new Fridges
                    {
                        Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991470"),
                        Name = "JH",
                        Owner_Name = "Rakun",
                        ModelId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991879"),
                    }
                );

        }
    }
}
