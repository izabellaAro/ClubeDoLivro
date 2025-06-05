using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ReadingScheduleConfig : IEntityTypeConfiguration<ReadingSchedule>
{
    public void Configure(EntityTypeBuilder<ReadingSchedule> builder)
    {
        builder
            .HasOne(a => a.Livro)
            .WithMany()
            .HasForeignKey(a => a.LivroId);
    }
}