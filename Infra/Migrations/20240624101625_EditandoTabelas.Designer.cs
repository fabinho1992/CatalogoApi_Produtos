﻿// <auto-generated />
using System;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infraestrutura.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20240624101625_EditandoTabelas")]
    partial class EditandoTabelas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dominio.Modelos.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagemUrl")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id")
                        .HasName("CategoriaId");

                    b.ToTable("Categorias", (string)null);
                });

            modelBuilder.Entity("Dominio.Modelos.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DataCadastro")
                        .HasColumnType("date");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<float>("Estoque")
                        .HasColumnType("real");

                    b.Property<string>("ImagemUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR");

                    b.Property<double>("Preco")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("ProdutoId");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Produtos", (string)null);
                });

            modelBuilder.Entity("Dominio.Modelos.Produto", b =>
                {
                    b.HasOne("Dominio.Modelos.Categoria", "Categoria")
                        .WithMany("Produtos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Dominio.Modelos.Categoria", b =>
                {
                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
