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
    [Migration("20241205170647_CLOUD")]
    partial class CLOUD
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("ProjectSequence");

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

            modelBuilder.Entity("StaticProjectStaticTemplate", b =>
                {
                    b.Property<int>("ProjectsId")
                        .HasColumnType("int");

                    b.Property<int>("StaticTemplatesId")
                        .HasColumnType("int");

                    b.HasKey("ProjectsId", "StaticTemplatesId");

                    b.HasIndex("StaticTemplatesId");

                    b.ToTable("StaticProjectStaticTemplate");
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
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasBeenPaid")
                        .HasColumnType("bit");

                    b.Property<bool>("HasDelivered")
                        .HasColumnType("bit");

                    b.Property<float>("Salary")
                        .HasColumnType("real");

                    b.Property<int>("ShipmentId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId", "CreatorId");

                    b.HasIndex("CreatorId");

                    b.ToTable("CreatorParticipations");
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

            modelBuilder.Entity("WCPShared.Models.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Features")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FocusPoints")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HowToUse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pains")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR [ProjectSequence]");

                    SqlServerPropertyBuilderExtensions.UseSequence(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Files")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Formats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InternalNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Platforms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("ProductId");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
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

            modelBuilder.Entity("WCPShared.Models.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastPaid")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfBrands")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfUsers")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfVideos")
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.Creator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PriceEstimate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeAccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tags")
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

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsoCountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsoLanguageCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

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

                    b.HasIndex("LanguageId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.CreatorProject", b =>
                {
                    b.HasBaseType("WCPShared.Models.Entities.ProjectModels.Project");

                    b.Property<int>("CreativesPerCreator")
                        .HasColumnType("int");

                    b.Property<string>("CreatorAge")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorBudget")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatorCount")
                        .HasColumnType("int");

                    b.Property<string>("CreatorGender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CreatorKeepsProduct")
                        .HasColumnType("bit");

                    b.Property<int>("CreatorLanguageId")
                        .HasColumnType("int");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("CreatorLanguageId");

                    b.ToTable((string)null);
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.StaticProject", b =>
                {
                    b.HasBaseType("WCPShared.Models.Entities.ProjectModels.Project");

                    b.ToTable("StaticProjects");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.PhotoProject", b =>
                {
                    b.HasBaseType("WCPShared.Models.Entities.ProjectModels.CreatorProject");

                    b.ToTable("PhotoProjects");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.UgcProject", b =>
                {
                    b.HasBaseType("WCPShared.Models.Entities.ProjectModels.CreatorProject");

                    b.Property<int>("ExtraHooks")
                        .HasColumnType("int");

                    b.Property<int>("LengthInSeconds")
                        .HasColumnType("int");

                    b.ToTable("UgcProjects");
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

            modelBuilder.Entity("StaticProjectStaticTemplate", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.ProjectModels.StaticProject", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
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

                    b.HasOne("WCPShared.Models.Entities.ProjectModels.CreatorProject", "Project")
                        .WithMany("Participations")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Project");
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

            modelBuilder.Entity("WCPShared.Models.Entities.Product", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.Project", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Brand", "Brand")
                        .WithMany("Projects")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WCPShared.Models.Entities.Product", "Product")
                        .WithMany("Projects")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Subscription", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.Organization", "Organization")
                        .WithOne("Subscription")
                        .HasForeignKey("WCPShared.Models.Entities.Subscription", "OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
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
                    b.HasOne("WCPShared.Models.Entities.UserModels.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WCPShared.Models.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Language");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.CreatorProject", b =>
                {
                    b.HasOne("WCPShared.Models.Entities.UserModels.Language", "CreatorLanguage")
                        .WithMany()
                        .HasForeignKey("CreatorLanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatorLanguage");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Brand", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Projects");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Organization", b =>
                {
                    b.Navigation("Brands");

                    b.Navigation("Subscription")
                        .IsRequired();
                });

            modelBuilder.Entity("WCPShared.Models.Entities.Product", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.UserModels.Creator", b =>
                {
                    b.Navigation("Participations");
                });

            modelBuilder.Entity("WCPShared.Models.Entities.ProjectModels.CreatorProject", b =>
                {
                    b.Navigation("Participations");
                });
#pragma warning restore 612, 618
        }
    }
}
