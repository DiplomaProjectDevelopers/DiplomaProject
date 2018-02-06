using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DiplomaProject.Domain.Entities
{
    public class DiplomaProjectContext : IdentityDbContext<User>
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

        //public virtual DbSet<UserRole> Roles { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=DiplomaProject_dev;Trusted_Connection=True;");
        //    }
        //}

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
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Branch>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(100);
            //});

            //modelBuilder.Entity<Department>(entity =>
            //{
            //    entity.Property(e => e.Description).HasMaxLength(400);

            //    entity.Property(e => e.Email).HasMaxLength(100);

            //    entity.Property(e => e.HeadOfDepartment).HasMaxLength(150);

            //    entity.Property(e => e.Name).HasMaxLength(150);

            //    entity.Property(e => e.Phone).HasMaxLength(50);

            //    entity.HasOne(d => d.Faculty)
            //        .WithMany(p => p.Departments)
            //        .HasForeignKey(d => d.FacultyId)
            //        .HasConstraintName("FK_Departments_Faculties");
            //});

            //modelBuilder.Entity<Edge>(entity =>
            //{
            //    entity.HasOne(d => d.FromNodeNavigation)
            //        .WithMany(p => p.EdgeFromNodeNavigation)
            //        .HasForeignKey(d => d.FromNode)
            //        .HasConstraintName("FK_Edge_EndResult");

            //    entity.HasOne(d => d.ToNodeNavigation)
            //        .WithMany(p => p.EdgeToNodeNavigation)
            //        .HasForeignKey(d => d.ToNode)
            //        .HasConstraintName("FK_Edge_OutCome1");
            //});

            //modelBuilder.Entity<Faculty>(entity =>
            //{
            //    entity.Property(e => e.Description).HasMaxLength(400);

            //    entity.Property(e => e.Director).HasMaxLength(200);

            //    entity.Property(e => e.Email).HasMaxLength(100);

            //    entity.Property(e => e.Name).HasMaxLength(200);

            //    entity.Property(e => e.Phone).HasMaxLength(50);
            //});

            //modelBuilder.Entity<FinalOutCome>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(200);

            //    entity.HasOne(d => d.Subject)
            //        .WithMany(p => p.FinalOutComes)
            //        .HasForeignKey(d => d.SubjectId)
            //        .HasConstraintName("FK_FinalOutComes_Subject");
            //});

            //modelBuilder.Entity<InitialOutCome>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(150);

            //    entity.HasOne(d => d.Subject)
            //        .WithMany(p => p.InitialOutComes)
            //        .HasForeignKey(d => d.SubjectId)
            //        .HasConstraintName("FK_InitialOutComes_InitialSubject");

            //    entity.HasOne(d => d.Type)
            //        .WithMany(p => p.InitialOutComes)
            //        .HasForeignKey(d => d.TypeId)
            //        .HasConstraintName("FK_InitialOutComes_OutComeType");
            //});

            //modelBuilder.Entity<InitialSubject>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(200);

            //    entity.HasOne(d => d.Profession)
            //        .WithMany(p => p.InitialSubject)
            //        .HasForeignKey(d => d.ProfessionId)
            //        .HasConstraintName("FK_InitialSubject_Professions");
            //});

            //modelBuilder.Entity<OutCome>(entity =>
            //{
            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(200);

            //    entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");

            //    entity.HasOne(d => d.Profession)
            //        .WithMany(p => p.OutCome)
            //        .HasForeignKey(d => d.ProfessionId)
            //        .HasConstraintName("FK_OutCome_Professions");

            //    entity.HasOne(d => d.Stakeholder)
            //        .WithMany(p => p.OutCome)
            //        .HasForeignKey(d => d.StakeholderId)
            //        .HasConstraintName("FK_EndResult_StakeHolder");

            //    entity.HasOne(d => d.Subject)
            //        .WithMany(p => p.OutCome)
            //        .HasForeignKey(d => d.SubjectId)
            //        .HasConstraintName("FK_OutCome_InitialSubject");

            //    entity.HasOne(d => d.Type)
            //        .WithMany(p => p.OutCome)
            //        .HasForeignKey(d => d.TypeId)
            //        .HasConstraintName("FK_Outcome_OutcomeType");
            //});

            //modelBuilder.Entity<OutComeType>(entity =>
            //{
            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(100);
            //});

            //modelBuilder.Entity<Profession>(entity =>
            //{
            //    entity.Property(e => e.BdfullTime).HasColumnName("BDFullTime");

            //    entity.Property(e => e.BdfullTimeSemesters).HasColumnName("BDFullTimeSemesters");

            //    entity.Property(e => e.BdpartTime).HasColumnName("BDPartTime");

            //    entity.Property(e => e.BdpartTimeSemesters).HasColumnName("BDPartTimeSemesters");

            //    entity.Property(e => e.Description).HasMaxLength(300);

            //    entity.Property(e => e.MdfullTime).HasColumnName("MDFullTime");

            //    entity.Property(e => e.MdfullTimeSemesters).HasColumnName("MDFullTimeSemesters");

            //    entity.Property(e => e.MdpartTime).HasColumnName("MDPartTime");

            //    entity.Property(e => e.MdpartTimeSemesters).HasColumnName("MDPartTimeSemesters");

            //    entity.Property(e => e.Name).HasMaxLength(50);

            //    entity.HasOne(d => d.Admin)
            //        .WithMany(p => p.Professions)
            //        .HasForeignKey(d => d.AdminId)
            //        .HasConstraintName("FK_Professions_ProfessionAdmins");

            //    entity.HasOne(d => d.Department)
            //        .WithMany(p => p.Professions)
            //        .HasForeignKey(d => d.DepartmentId)
            //        .HasConstraintName("FK_Professions_Departments");
            //});

            //modelBuilder.Entity<StakeHolder>(entity =>
            //{
            //    entity.Property(e => e.CompanyName).HasMaxLength(250);

            //    entity.Property(e => e.Email).HasMaxLength(150);

            //    entity.Property(e => e.FirstName).HasMaxLength(50);

            //    entity.Property(e => e.LastName).HasMaxLength(50);

            //    entity.Property(e => e.Phone).HasMaxLength(50);

            //    entity.HasOne(d => d.Branch)
            //        .WithMany(p => p.StakeHolder)
            //        .HasForeignKey(d => d.BranchId)
            //        .HasConstraintName("FK_StakeHolder_Branches");

            //    entity.HasOne(d => d.Type)
            //        .WithMany(p => p.StakeHolder)
            //        .HasForeignKey(d => d.TypeId)
            //        .HasConstraintName("FK_StakeHolder_StakeHolderType");
            //});

            //modelBuilder.Entity<StakeHolderType>(entity =>
            //{
            //    entity.Property(e => e.ProfessionName).HasMaxLength(100);

            //    entity.Property(e => e.TypeName).HasMaxLength(100);
            //});

            //modelBuilder.Entity<Subject>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(200);
            //});

            //modelBuilder.Entity<UserRole>(entity =>
            //{
            //    entity.Property(e => e.Name).HasMaxLength(250);
            //});

            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.Property(e => e.Email).HasMaxLength(100);

            //    entity.Property(e => e.FirstName).HasMaxLength(20);

            //    entity.Property(e => e.LastName).HasMaxLength(30);

            //    entity.Property(e => e.Password).HasMaxLength(500);

            //    entity.Property(e => e.Phone).HasMaxLength(150);

            //    entity.Property(e => e.UserName).HasMaxLength(30);
            //});
        }
    }
}
