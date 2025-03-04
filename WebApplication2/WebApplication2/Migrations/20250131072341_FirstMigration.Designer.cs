﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication2.Entities;

#nullable disable

namespace WebApplication2.Migrations
{
    [DbContext(typeof(Db1Context))]
    [Migration("20250131072341_FirstMigration")]
    partial class FirstMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("WebApplication2.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Id" }, "id_UNIQUE")
                        .IsUnique();

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("WebApplication2.Entities.Product", b =>
                {
                    b.Property<uint>("Idproduct")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("idproduct");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<uint>("Idproduct"));

                    b.Property<int>("CategoriesId")
                        .HasColumnType("int")
                        .HasColumnName("categories_id");

                    b.Property<byte[]>("Image")
                        .HasColumnType("longblob")
                        .HasColumnName("image");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("title");

                    b.Property<string>("TitlePath")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("title_path");

                    b.HasKey("Idproduct")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CategoriesId" }, "fk_product_categories_idx");

                    b.HasIndex(new[] { "Idproduct" }, "idproduct_UNIQUE")
                        .IsUnique();

                    b.ToTable("product", (string)null);
                });

            modelBuilder.Entity("WebApplication2.Entities.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idUser");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("password");

                    b.HasKey("IdUser")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Email" }, "email_UNIQUE")
                        .IsUnique();

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("WebApplication2.Entities.Product", b =>
                {
                    b.HasOne("WebApplication2.Entities.Category", "Categories")
                        .WithMany("Products")
                        .HasForeignKey("CategoriesId")
                        .IsRequired()
                        .HasConstraintName("fk_product_categories");

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("WebApplication2.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
