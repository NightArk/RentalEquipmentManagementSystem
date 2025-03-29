using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class EquipmentRentalDBContext : DbContext
{
    public EquipmentRentalDBContext()
    {
    }

    public EquipmentRentalDBContext(DbContextOptions<EquipmentRentalDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<RentalRequest> RentalRequests { get; set; }

    public virtual DbSet<RentalTransaction> RentalTransactions { get; set; }

    public virtual DbSet<ReturnRecord> ReturnRecords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EquipmentRentalDB;Trusted_Connection=True;");
#pragma warning restore CS1030 // #warning directive

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0793894664");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC071DFE5247");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.RentalTransaction).WithMany(p => p.Documents).HasConstraintName("FK__Documents__Renta__76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.Documents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Documents__UserI__778AC167");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC071ABD2FEB");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Equipment)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Equipment__Categ__3E52440B");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07DBF5D9A1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Feedback__Equipm__6D0D32F4");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Feedback__UserId__6C190EBB");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC07E1A71C35");

            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Logs__UserId__02084FDA");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC075D4B83A5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Unread");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notificat__UserI__7B5B524B");
        });

        modelBuilder.Entity<RentalRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RentalRe__3214EC0744D686FB");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalRequests)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RentalReq__Custo__440B1D61");

            entity.HasOne(d => d.Equipment).WithMany(p => p.RentalRequests)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RentalReq__Equip__44FF419A");
        });

        modelBuilder.Entity<RentalTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RentalTr__3214EC0771B36456");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentStatus).HasDefaultValue("Pending");

            entity.HasOne(d => d.AssignedEquipment).WithMany(p => p.RentalTransactions).HasConstraintName("FK__RentalTra__Assig__5EBF139D");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalTransactions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RentalTra__Custo__5FB337D6");

            entity.HasOne(d => d.RentalRequest).WithMany(p => p.RentalTransactions).HasConstraintName("FK__RentalTra__Renta__5DCAEF64");
        });

        modelBuilder.Entity<ReturnRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReturnRe__3214EC07E6EFC33C");

            entity.Property(e => e.AdditionalCharges).HasDefaultValue(0.00m);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LateReturnFee).HasDefaultValue(0.00m);

            entity.HasOne(d => d.RentalTransaction).WithMany(p => p.ReturnRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ReturnRec__Renta__656C112C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0737430413");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
