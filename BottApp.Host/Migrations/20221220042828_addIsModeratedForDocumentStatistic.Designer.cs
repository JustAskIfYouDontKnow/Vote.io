﻿// <auto-generated />
using System;
using BottApp.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BottApp.Host.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    [Migration("20221220042828_addIsModeratedForDocumentStatistic")]
    partial class addIsModeratedForDocumentStatistic
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BottApp.Database.Document.DocumentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Caption")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DocumentExtension")
                        .HasColumnType("text");

                    b.Property<int>("DocumentInPath")
                        .HasColumnType("integer");

                    b.Property<int?>("DocumentNomination")
                        .HasColumnType("integer");

                    b.Property<string>("DocumentType")
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("BottApp.Database.Document.Like.LikedDocumentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<bool>("isLiked")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("LikedDocument");
                });

            modelBuilder.Entity("BottApp.Database.Document.Statistic.DocumentStatisticModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsModerated")
                        .HasColumnType("boolean");

                    b.Property<int>("LikeCount")
                        .HasColumnType("integer");

                    b.Property<int>("ViewCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("DocumentStatistic");
                });

            modelBuilder.Entity("BottApp.Database.User.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<int>("OnState")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("TelegramFirstName")
                        .HasColumnType("text");

                    b.Property<string>("TelegramLastName")
                        .HasColumnType("text");

                    b.Property<long>("UId")
                        .HasColumnType("bigint");

                    b.Property<int>("ViewDocumentID")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UId")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("BottApp.Database.UserMessage.MessageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("BottApp.Database.Document.DocumentModel", b =>
                {
                    b.HasOne("BottApp.Database.User.UserModel", "UserModel")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserModel");
                });

            modelBuilder.Entity("BottApp.Database.Document.Like.LikedDocumentModel", b =>
                {
                    b.HasOne("BottApp.Database.Document.DocumentModel", "DocumentModel")
                        .WithMany("Likes")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentModel");
                });

            modelBuilder.Entity("BottApp.Database.Document.Statistic.DocumentStatisticModel", b =>
                {
                    b.HasOne("BottApp.Database.Document.DocumentModel", "DocumentModel")
                        .WithOne("DocumentStatisticModel")
                        .HasForeignKey("BottApp.Database.Document.Statistic.DocumentStatisticModel", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentModel");
                });

            modelBuilder.Entity("BottApp.Database.UserMessage.MessageModel", b =>
                {
                    b.HasOne("BottApp.Database.User.UserModel", "UserModel")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserModel");
                });

            modelBuilder.Entity("BottApp.Database.Document.DocumentModel", b =>
                {
                    b.Navigation("DocumentStatisticModel")
                        .IsRequired();

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("BottApp.Database.User.UserModel", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
