﻿// <auto-generated />
using DiplomaProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DiplomaProject.Domain.Migrations
{
    [DbContext(typeof(DiplomaProjectContext))]
    partial class DiplomaProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte?>("CourseWorkMaxCount");

                    b.Property<byte?>("CreditMaxCount");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<byte?>("ExamMaxCount");

                    b.Property<int?>("FacultyId");

                    b.Property<string>("HeadOfDepartment");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<byte?>("TestMaxCount");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Edge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LeftOutComeId");

                    b.Property<int>("ProfessionId");

                    b.Property<int?>("RightOutComeId");

                    b.HasKey("Id");

                    b.HasIndex("LeftOutComeId");

                    b.HasIndex("RightOutComeId");

                    b.ToTable("Edges");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Director");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.FinalOutCome", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("IsNew");

                    b.Property<string>("Name");

                    b.Property<int?>("OutComeTypeId");

                    b.Property<int?>("ProfessionId");

                    b.Property<int?>("SubjectId");

                    b.Property<double?>("TotalWeight");

                    b.Property<int?>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("OutComeTypeId");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("SubjectId");

                    b.ToTable("FinalOutComes");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.InitialOutCome", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("SubjectId");

                    b.Property<int?>("TypeId");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TypeId");

                    b.ToTable("InitialOutComes");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.InitialSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("ProfessionId");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.ToTable("InitialSubjects");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.OutCome", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("IsNew");

                    b.Property<string>("Name");

                    b.Property<int?>("ProfessionId");

                    b.Property<int?>("StakeholderId");

                    b.Property<int?>("SubjectId");

                    b.Property<int?>("TypeId");

                    b.Property<decimal>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("StakeholderId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TypeId");

                    b.ToTable("OutComes");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.OutComeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("OutComeTypes");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Profession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminId");

                    b.Property<bool?>("BdfullTime");

                    b.Property<byte?>("BdfullTimeSemesters");

                    b.Property<bool?>("BdpartTime");

                    b.Property<byte?>("BdpartTimeSemesters");

                    b.Property<int?>("BranchId");

                    b.Property<int?>("DepartmentId");

                    b.Property<string>("Description");

                    b.Property<bool?>("MdfullTime");

                    b.Property<byte?>("MdfullTimeSemesters");

                    b.Property<bool?>("MdpartTime");

                    b.Property<byte?>("MdpartTimeSemesters");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("BranchId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<int?>("Priority");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.StakeHolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BranchId");

                    b.Property<string>("CompanyName");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Phone");

                    b.Property<int?>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("TypeId");

                    b.ToTable("StakeHolders");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.StakeHolderType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double?>("Coefficient");

                    b.Property<string>("ProfessionName");

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("StakeHolderTypes");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseHourse");

                    b.Property<int?>("Credit");

                    b.Property<int?>("LabHours");

                    b.Property<int?>("LectionHours");

                    b.Property<int>("Level");

                    b.Property<int?>("ModuleId");

                    b.Property<string>("Name");

                    b.Property<int?>("PracticalHours");

                    b.Property<int?>("ProfessionId");

                    b.Property<int?>("SubjectModuleId");

                    b.Property<int?>("TotalHours");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("SubjectModuleId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.SubjectModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("SubjectModules");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500);

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool?>("Gender");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.UserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Department", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Faculty", "Faculty")
                        .WithMany("Departments")
                        .HasForeignKey("FacultyId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Edge", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.FinalOutCome", "LeftOutCome")
                        .WithMany("LeftSideOutComes")
                        .HasForeignKey("LeftOutComeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DiplomaProject.Domain.Entities.FinalOutCome", "RightOutCome")
                        .WithMany("RightSideOutComes")
                        .HasForeignKey("RightOutComeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.FinalOutCome", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.OutComeType", "OutComeType")
                        .WithMany("FinalOutComes")
                        .HasForeignKey("OutComeTypeId");

                    b.HasOne("DiplomaProject.Domain.Entities.Profession", "Profession")
                        .WithMany()
                        .HasForeignKey("ProfessionId");

                    b.HasOne("DiplomaProject.Domain.Entities.Subject", "Subject")
                        .WithMany("FinalOutComes")
                        .HasForeignKey("SubjectId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.InitialOutCome", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.InitialSubject", "Subject")
                        .WithMany("InitialOutComes")
                        .HasForeignKey("SubjectId");

                    b.HasOne("DiplomaProject.Domain.Entities.OutComeType", "Type")
                        .WithMany("InitialOutComes")
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.InitialSubject", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Profession", "Profession")
                        .WithMany("InitialSubject")
                        .HasForeignKey("ProfessionId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.OutCome", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Profession", "Profession")
                        .WithMany("OutCome")
                        .HasForeignKey("ProfessionId");

                    b.HasOne("DiplomaProject.Domain.Entities.StakeHolder", "Stakeholder")
                        .WithMany("OutCome")
                        .HasForeignKey("StakeholderId");

                    b.HasOne("DiplomaProject.Domain.Entities.InitialSubject", "Subject")
                        .WithMany("OutComes")
                        .HasForeignKey("SubjectId");

                    b.HasOne("DiplomaProject.Domain.Entities.OutComeType", "Type")
                        .WithMany("OutComes")
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Profession", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.User", "Admin")
                        .WithMany("Professions")
                        .HasForeignKey("AdminId");

                    b.HasOne("DiplomaProject.Domain.Entities.Branch", "Branch")
                        .WithMany("Professions")
                        .HasForeignKey("BranchId");

                    b.HasOne("DiplomaProject.Domain.Entities.Department", "Department")
                        .WithMany("Professions")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.StakeHolder", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Branch", "Branch")
                        .WithMany("StakeHolder")
                        .HasForeignKey("BranchId");

                    b.HasOne("DiplomaProject.Domain.Entities.StakeHolderType", "Type")
                        .WithMany("StakeHolder")
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.Subject", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Profession", "Profession")
                        .WithMany()
                        .HasForeignKey("ProfessionId");

                    b.HasOne("DiplomaProject.Domain.Entities.SubjectModule", "SubjectModule")
                        .WithMany("Subjects")
                        .HasForeignKey("SubjectModuleId");
                });

            modelBuilder.Entity("DiplomaProject.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DiplomaProject.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DiplomaProject.Domain.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
