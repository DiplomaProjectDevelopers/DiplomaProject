using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DiplomaProject.Domain.Entities
{
    public class DiplomaProjectContext 
        : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {         
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
            modelBuilder.Entity<IdentityUserRole<string>>().HasDiscriminator().HasValue("Text");
            base.OnModelCreating(modelBuilder);
        }
    }
}
