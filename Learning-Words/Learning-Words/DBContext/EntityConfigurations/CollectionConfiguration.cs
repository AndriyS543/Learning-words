using Microsoft.EntityFrameworkCore;
using Learning_Words.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning_Words.DBContext.EntityConfigurations
{
    class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(c => c.Chapters)
                   .WithOne(ch => ch.Collection)
                   .HasForeignKey(ch => ch.CollectionId);
        }
    }
}
