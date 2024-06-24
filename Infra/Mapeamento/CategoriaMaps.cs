using Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Mapeamento
{
    public class CategoriaMaps : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(x => x.Id).HasName("CategoriaId");

            builder.Property(c => c.Nome)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.ImagemUrl)
                .HasColumnType("VARCHAR")
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}
