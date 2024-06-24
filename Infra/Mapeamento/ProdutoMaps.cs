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
    public class ProdutoMaps : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(x => x.Id).HasName("ProdutoId");

            builder.Property(p => p.Nome)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasColumnType("VARCHAR")
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
