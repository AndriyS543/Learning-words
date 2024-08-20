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
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.NameWord).IsRequired().HasMaxLength(50);
            builder.Property(w => w.Translation).IsRequired().HasMaxLength(50);
            builder.Property(w => w.Transcription).HasMaxLength(50);
            builder.Ignore(w => w.IsCorrect);
            builder.HasOne(w => w.Chapter)
                   .WithMany(ch => ch.Words)
                   .HasForeignKey(w => w.ChapterId);
        }
    }
}
