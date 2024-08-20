using Learning_Words.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.DBContext.EntityConfigurations
{
    class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(ch => ch.Id);
            builder.Property(ch => ch.NameChapter).IsRequired().HasMaxLength(50);
            builder.Property(ch => ch.Description).HasMaxLength(100);
            builder.Ignore(ch=>ch.IsUsed);
            builder.HasOne(ch => ch.Collection)
                   .WithMany(c => c.Chapters)
                   .HasForeignKey(ch => ch.CollectionId);
        }
    }
}
