﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WCPShared.Models;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20241106152149_Stripe")]
    partial class Stripe
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CreatorLanguage", b =>
                {
                    b.Property<int>("LanguagesId")
                        .HasColumnType("int");

                    b.Property<int>("SpeakersId")
                        .HasColumnType("int");

                    b.HasKey("LanguagesId", "SpeakersId");

                    b.HasIndex("SpeakersId");

                    b.ToTable("CreatorLanguage");
                });

            modelBuilder.Entity("OrderStaticTemplate", b =>
                {
                    b.Property<int>("OrdersId")
                        .HasColumnType("int");

                    b.Property<int>("StaticTemplatesId")
                        .HasColumnType("int");

                    b.HasKey("OrdersId", "StaticTemplatesId");

                    b.HasIndex("StaticTemplatesId");

                    b.ToTable("OrderStaticTemplate");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.CreatorParticipation", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<bool>("HasDelivered")
                        .HasColumnType("bit");

                    b.Property<float>("Salary")
                        .HasColumnType("real");

                    b.HasKey("OrderId", "CreatorId");

                    b.HasIndex("CreatorId");

                    b.ToTable("CreatorParticipations");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContentCount")
                        .HasColumnType("int");

                    b.Property<int?>("ContentLength")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Delivery")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryTimeFrom")
                        .HasColumnType("int");

                    b.Property<int>("DeliveryTimeTo")
                        .HasColumnType("int");

                    b.Property<int?>("EditorId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ExtraCreator")
                        .HasColumnType("bit");

                    b.Property<int?>("ExtraHook")
                        .HasColumnType("int");

                    b.Property<string>("ExtraNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FocusPoints")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ideas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InternalNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Other")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Platforms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Products")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelevantFiles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Scripts")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VideographerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("EditorId");

                    b.HasIndex("VideographerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CVR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.StaticTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExampleImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateImgOne")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateImgTwo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StaticTemplates");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.Creator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriceEstimate")
                        .HasColumnType("int");

                    b.Property<string>("StripeAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Creators");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginAttempt")
                        .HasColumnType("datetime2");

                    b.Property<int>("LoginTries")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotificationSetting")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NotificationsOn")
                        .HasColumnType("bit");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ResetTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CreatorLanguage", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.UserModels.Language", null)
                        .WithMany()
                        .HasForeignKey("LanguagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WCPShared.Models.Entities.UserModels.Creator", null)
                        .WithMany()
                        .HasForeignKey("SpeakersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrderStaticTemplate", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WCPShared.Models.Entities.StaticTemplate", null)
                        .WithMany()
                        .HasForeignKey("StaticTemplatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Brand", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Organization", "Organization")
                        .WithMany("Brands")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.CreatorParticipation", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.UserModels.Creator", "Creator")
                        .WithMany("Participations")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WCPShared.Models.Entities.Order", "Order")
                        .WithMany("Participations")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Order", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Brand", "Brand")
                        .WithMany("Orders")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WCPShared.Models.Entities.UserModels.Creator", "Editor")
                        .WithMany()
                        .HasForeignKey("EditorId");

                    b.HasOne("WCPShared.Models.Entities.UserModels.Creator", "Videographer")
                        .WithMany()
                        .HasForeignKey("VideographerId");

                    b.Navigation("Brand");

                    b.Navigation("Editor");

                    b.Navigation("Videographer");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Organization", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.UserModels.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.Creator", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.UserModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.User", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Brand", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Order", b =>
                {
                    b.Navigation("Participations");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Organization", b =>
                {
                    b.Navigation("Brands");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.Creator", b =>
                {
                    b.Navigation("Participations");
                });
#pragma warning restore 612, 618
        }
    }
}
