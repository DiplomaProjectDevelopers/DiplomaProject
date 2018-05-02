using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DiplomaProject.Domain.Entities
{
    public class DiplomaProjectContext
        : IdentityDbContext<User,Role,string,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {
        public DiplomaProjectContext(DbContextOptions<DiplomaProjectContext> options) : base(options)
        {
        }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Edge> Edges { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<FinalOutCome> FinalOutComes { get; set; }
        public virtual DbSet<InitialOutCome> InitialOutComes { get; set; }
        public virtual DbSet<InitialSubject> InitialSubjects { get; set; }
        public virtual DbSet<OutCome> OutComes { get; set; }
        public virtual DbSet<OutComeType> OutComeTypes { get; set; }
        public virtual DbSet<Profession> Professions { get; set; }
        public virtual DbSet<StakeHolder> StakeHolders { get; set; }
        public virtual DbSet<StakeHolderType> StakeHolderTypes { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectModule> SubjectModules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Role>(b =>
            //{
            //    b.HasKey(r => r.Id);
            //    b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
            //    b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            //    b.Property(u => u.Name).HasMaxLength(256);
            //    b.Property(u => u.NormalizedName).HasMaxLength(256);

            //    b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            //    b.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            //});
            //modelBuilder.Entity<UserRole>(entity =>
            //{
            //    entity.HasKey(m => new { m.UserId, m.RoleId, m.ProfessionId });
            //});
            modelBuilder.Entity<Edge>()
               .HasOne(m => m.LeftOutCome)
               .WithMany(t => t.LeftSideOutComes)
               .HasForeignKey(m => m.LeftOutComeId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Edge>()
                .HasOne(m => m.RightOutCome)
                .WithMany(t => t.RightSideOutComes)
                .HasForeignKey(m => m.RightOutComeId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
            });
            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                var mutableKeys = entity.Metadata.GetKeys().ToList();
                for (int i = 0; i < mutableKeys.Count; i++)
                {
                    entity.Metadata.RemoveKey(mutableKeys[i].Properties);
                }

                entity.HasKey(m => new { m.UserId, m.RoleId, m.ProfessionId });
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
            });
            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
        }
    }
}
