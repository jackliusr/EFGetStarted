﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFGetStarted.Migrations
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20241031112552_ef sample")]
    partial class efsample
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SiteUri")
                        .IsUnique();

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<int>("BlogId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<DateOnly>("PublishedOn")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts");

                    b.HasDiscriminator().HasValue("Post");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.Property<int>("PostsId")
                        .HasColumnType("integer");

                    b.Property<string>("TagsId")
                        .HasColumnType("text");

                    b.HasKey("PostsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("Tag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Website", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Website");
                });

            modelBuilder.Entity("FeaturedPost", b =>
                {
                    b.HasBaseType("Post");

                    b.Property<string>("PromoText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("FeaturedPost");
                });

            modelBuilder.Entity("Author", b =>
                {
                    b.OwnsOne("ContactDetails", "Contact", b1 =>
                        {
                            b1.Property<int>("AuthorId")
                                .HasColumnType("integer");

                            b1.Property<string>("Phone")
                                .HasColumnType("text");

                            b1.HasKey("AuthorId");

                            b1.ToTable("Author");

                            b1.ToJson("Contact");

                            b1.WithOwner()
                                .HasForeignKey("AuthorId");

                            b1.OwnsOne("Address", "Address", b2 =>
                                {
                                    b2.Property<int>("ContactDetailsAuthorId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("City")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Country")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Postcode")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Street")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("ContactDetailsAuthorId");

                                    b2.ToTable("Author");

                                    b2.WithOwner()
                                        .HasForeignKey("ContactDetailsAuthorId");
                                });

                            b1.Navigation("Address")
                                .IsRequired();
                        });

                    b.Navigation("Contact")
                        .IsRequired();
                });

            modelBuilder.Entity("Blog", b =>
                {
                    b.HasOne("Website", "Site")
                        .WithOne("Blog")
                        .HasForeignKey("Blog", "SiteUri")
                        .HasPrincipalKey("Website", "Uri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Site");
                });

            modelBuilder.Entity("Post", b =>
                {
                    b.HasOne("Author", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId");

                    b.HasOne("Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("PostMetadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("PostId")
                                .HasColumnType("integer");

                            b1.Property<int>("Views")
                                .HasColumnType("integer");

                            b1.HasKey("PostId");

                            b1.ToTable("Posts");

                            b1.ToJson("Metadata");

                            b1.WithOwner()
                                .HasForeignKey("PostId");

                            b1.OwnsMany("PostUpdate", "Updates", b2 =>
                                {
                                    b2.Property<int>("PostMetadataPostId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<IPAddress>("PostedFrom")
                                        .IsRequired()
                                        .HasColumnType("inet");

                                    b2.Property<string>("UpdatedBy")
                                        .HasColumnType("text");

                                    b2.Property<DateTime>("UpdatedOn")
                                        .HasColumnType("timestamp with time zone");

                                    b2.HasKey("PostMetadataPostId", "__synthesizedOrdinal");

                                    b2.ToTable("Posts");

                                    b2.WithOwner()
                                        .HasForeignKey("PostMetadataPostId");

                                    b2.OwnsMany("Commit", "Commits", b3 =>
                                        {
                                            b3.Property<int>("PostUpdatePostMetadataPostId")
                                                .HasColumnType("integer");

                                            b3.Property<int>("PostUpdate__synthesizedOrdinal")
                                                .HasColumnType("integer");

                                            b3.Property<int>("__synthesizedOrdinal")
                                                .ValueGeneratedOnAdd()
                                                .HasColumnType("integer");

                                            b3.Property<string>("Comment")
                                                .IsRequired()
                                                .HasColumnType("text");

                                            b3.Property<DateTime>("CommittedOn")
                                                .HasColumnType("timestamp with time zone");

                                            b3.HasKey("PostUpdatePostMetadataPostId", "PostUpdate__synthesizedOrdinal", "__synthesizedOrdinal");

                                            b3.ToTable("Posts");

                                            b3.WithOwner()
                                                .HasForeignKey("PostUpdatePostMetadataPostId", "PostUpdate__synthesizedOrdinal");
                                        });

                                    b2.Navigation("Commits");
                                });

                            b1.OwnsMany("SearchTerm", "TopSearches", b2 =>
                                {
                                    b2.Property<int>("PostMetadataPostId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<int>("Count")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Term")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("PostMetadataPostId", "__synthesizedOrdinal");

                                    b2.ToTable("Posts");

                                    b2.WithOwner()
                                        .HasForeignKey("PostMetadataPostId");
                                });

                            b1.OwnsMany("Visits", "TopGeographies", b2 =>
                                {
                                    b2.Property<int>("PostMetadataPostId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.PrimitiveCollection<List<string>>("Browsers")
                                        .HasColumnType("text[]");

                                    b2.Property<int>("Count")
                                        .HasColumnType("integer");

                                    b2.Property<double>("Latitude")
                                        .HasColumnType("double precision");

                                    b2.Property<double>("Longitude")
                                        .HasColumnType("double precision");

                                    b2.HasKey("PostMetadataPostId", "__synthesizedOrdinal");

                                    b2.ToTable("Posts");

                                    b2.WithOwner()
                                        .HasForeignKey("PostMetadataPostId");
                                });

                            b1.Navigation("TopGeographies");

                            b1.Navigation("TopSearches");

                            b1.Navigation("Updates");
                        });

                    b.Navigation("Author");

                    b.Navigation("Blog");

                    b.Navigation("Metadata");
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.HasOne("Post", null)
                        .WithMany()
                        .HasForeignKey("PostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Author", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Blog", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Website", b =>
                {
                    b.Navigation("Blog")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
