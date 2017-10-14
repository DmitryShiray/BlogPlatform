using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BlogPlatform.Domain;

namespace BlogPlatform.Domain.Migrations
{
    [DbContext(typeof(BlogPlatformContext))]
    partial class BlogPlatformContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<byte[]>("Password");

                    b.Property<byte[]>("Salt");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("LastDateModified");

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<int>("ArticleId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ArticleId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<int>("ArticleId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ArticleId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Article", b =>
                {
                    b.HasOne("BlogPlatform.Domain.Entities.Account", "Account")
                        .WithMany("Articles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Comment", b =>
                {
                    b.HasOne("BlogPlatform.Domain.Entities.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BlogPlatform.Domain.Entities.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlogPlatform.Domain.Entities.Rating", b =>
                {
                    b.HasOne("BlogPlatform.Domain.Entities.Account", "Account")
                        .WithMany("Ratings")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BlogPlatform.Domain.Entities.Article", "Article")
                        .WithMany("Ratings")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
