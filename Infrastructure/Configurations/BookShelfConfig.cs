using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BookShelfConfig : IEntityTypeConfiguration<BookShelf>
{
    public void Configure(EntityTypeBuilder<BookShelf> builder)
    {
        builder.HasKey(l => l.Id);
        builder
            .HasOne(l => l.Livro)
            .WithMany(l => l.Usuarios)
            .HasForeignKey(lu => lu.LivroId);
        builder
            .HasIndex(l => new { l.UsuarioId, l.LivroId })
            .IsUnique();
    }
}