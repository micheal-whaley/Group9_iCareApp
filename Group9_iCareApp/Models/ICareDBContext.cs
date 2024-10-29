using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Group9_iCareApp.Models;

public class iCAREDBContext : DbContext
{
    public iCAREDBContext()
    {
    }

    public virtual DbSet<DocumentMetadatum> DocumentMetadata { get; set; }

    public virtual DbSet<DrugsDictionary> DrugsDictionaries { get; set; }

    public virtual DbSet<GeoCode> GeoCodes { get; set; }

    public virtual DbSet<iCAREAdmin> iCAREAdmins { get; set; }

    public virtual DbSet<iCAREUser> iCAREUsers { get; set; }

    public virtual DbSet<iCAREWorker> iCAREWorkers { get; set; }

    public virtual DbSet<ModificationHistory> ModificationHistories { get; set; }

    public virtual DbSet<PatientRecord> PatientRecords { get; set; }

    public virtual DbSet<TreatmentRecord> TreatmentRecords { get; set; }

    public virtual DbSet<UserPassword> UserPasswords { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public iCAREDBContext(DbContextOptions<iCAREDBContext> options)
    : base(options)
    {
    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=CHRISTIAN;Initial Catalog=iCARE;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<DocumentMetadatum>(entity =>
    //        {
    //            entity.HasKey(e => e.DocId).HasName("PK__Document__3EF1888DDD7318E2");

    //            entity.Property(e => e.DocId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("DocID");
    //            entity.Property(e => e.DateOfCreation).HasColumnType("datetime");
    //            entity.Property(e => e.DocName)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //        });

    //        modelBuilder.Entity<DrugsDictionary>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__DrugsDic__3214EC270D13C263");

    //            entity.ToTable("DrugsDictionary");

    //            entity.Property(e => e.Id).HasColumnName("ID");
    //            entity.Property(e => e.Description).IsUnicode(false);
    //            entity.Property(e => e.Name).IsUnicode(false);
    //        });

    //        modelBuilder.Entity<GeoCode>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__GeoCodes__3214EC27C87E4192");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.Description).IsUnicode(false);
    //        });

    //        modelBuilder.Entity<iCAREAdmin>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__iCAREAdm__3214EC273905C464");

    //            entity.ToTable("iCAREAdmin");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.AdminEmail)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);

    //            entity.HasOne(d => d.IdNavigation).WithOne(p => p.iCAREAdmin)
    //                .HasForeignKey<iCAREAdmin>(d => d.Id)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__iCAREAdmin__ID__3C69FB99");
    //        });

    //        modelBuilder.Entity<iCAREUser>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__iCAREUse__3214EC275A117A47");

    //            entity.ToTable("iCAREUser");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.Name)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //        });

    //        modelBuilder.Entity<iCAREWorker>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__iCAREWor__3214EC27C2015F25");

    //            entity.ToTable("iCAREWorker");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.Profession)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);

    //            entity.HasOne(d => d.IdNavigation).WithOne(p => p.iCAREWorker)
    //                .HasForeignKey<iCAREWorker>(d => d.Id)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__iCAREWorker__ID__398D8EEE");
    //        });

    //        modelBuilder.Entity<ModificationHistory>(entity =>
    //        {
    //            entity.HasKey(e => e.ModificationId).HasName("PK__Modifica__A3FE5A12391D4EDB");

    //            entity.ToTable("ModificationHistory");

    //            entity.Property(e => e.ModificationId).HasColumnName("ModificationID");
    //            entity.Property(e => e.DateOfModification).HasColumnType("datetime");
    //            entity.Property(e => e.Description).IsUnicode(false);
    //            entity.Property(e => e.DocId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("DocID");
    //            entity.Property(e => e.WorkerId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("WorkerID");

    //            entity.HasOne(d => d.Doc).WithMany(p => p.ModificationHistories)
    //                .HasForeignKey(d => d.DocId)
    //                .HasConstraintName("FK__Modificat__DocID__4D94879B");

    //            entity.HasOne(d => d.Worker).WithMany(p => p.ModificationHistories)
    //                .HasForeignKey(d => d.WorkerId)
    //                .HasConstraintName("FK__Modificat__Worke__4E88ABD4");
    //        });

    //        modelBuilder.Entity<PatientRecord>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__PatientR__3214EC27268C669B");

    //            entity.ToTable("PatientRecord");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.Address)
    //                .HasMaxLength(255)
    //                .IsUnicode(false);
    //            entity.Property(e => e.BedId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("BedID");
    //            entity.Property(e => e.BloodGroup)
    //                .HasMaxLength(10)
    //                .IsUnicode(false);
    //            entity.Property(e => e.Name)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //            entity.Property(e => e.TreatmentArea)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //        });

    //        modelBuilder.Entity<TreatmentRecord>(entity =>
    //        {
    //            entity.HasKey(e => e.TreatmentId).HasName("PK__Treatmen__1A57B711ABC92DA2");

    //            entity.ToTable("TreatmentRecord");

    //            entity.Property(e => e.TreatmentId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("TreatmentID");
    //            entity.Property(e => e.Description).IsUnicode(false);
    //            entity.Property(e => e.PatientId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("PatientID");
    //            entity.Property(e => e.TreatmentDate).HasColumnType("datetime");
    //            entity.Property(e => e.WorkerId)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("WorkerID");

    //            entity.HasOne(d => d.Patient).WithMany(p => p.TreatmentRecords)
    //                .HasForeignKey(d => d.PatientId)
    //                .HasConstraintName("FK__Treatment__Patie__47DBAE45");

    //            entity.HasOne(d => d.Worker).WithMany(p => p.TreatmentRecords)
    //                .HasForeignKey(d => d.WorkerId)
    //                .HasConstraintName("FK__Treatment__Worke__48CFD27E");
    //        });

    //        modelBuilder.Entity<UserPassword>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__UserPass__3214EC2779F9858B");

    //            entity.ToTable("UserPassword");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.Password).IsUnicode(false);
    //            entity.Property(e => e.Username)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);

    //            entity.HasOne(d => d.IdNavigation).WithOne(p => p.UserPassword)
    //                .HasForeignKey<UserPassword>(d => d.Id)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__UserPassword__ID__3F466844");
    //        });

    //        modelBuilder.Entity<UserRole>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC27AC10000D");

    //            entity.ToTable("UserRole");

    //            entity.Property(e => e.Id)
    //                .HasMaxLength(50)
    //                .IsUnicode(false)
    //                .HasColumnName("ID");
    //            entity.Property(e => e.RoleName)
    //                .HasMaxLength(100)
    //                .IsUnicode(false);
    //        });

    //        OnModelCreatingPartial(modelBuilder);
    //    }

    //    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
