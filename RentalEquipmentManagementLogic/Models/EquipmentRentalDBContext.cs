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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EquipmentRentalDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC077738D4D1");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07F41E3AE8");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.RentalTransaction).WithMany(p => p.Documents).HasConstraintName("FK__Documents__Renta__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.Documents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Documents__UserI__45F365D3");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC0700526B15");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Equipment)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Equipment__Categ__46E78A0C");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07FD2B3EFB");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Feedback__Equipm__47DBAE45");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Feedback__UserId__48CFD27E");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC070354FB99");

            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Logs__UserId__49C3F6B7");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07B4FDD2BC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Unread");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notificat__UserI__4AB81AF0");
        });

        modelBuilder.Entity<RentalRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RentalRe__3214EC07337EA2E8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalRequests)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RentalReq__Custo__4BAC3F29");

            entity.HasOne(d => d.Equipment).WithMany(p => p.RentalRequests)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RentalReq__Equip__4CA06362");
        });

        modelBuilder.Entity<RentalTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RentalTr__3214EC075B292D7C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentStatus).HasDefaultValue("Pending");

            entity.HasOne(d => d.AssignedEquipment).WithMany(p => p.RentalTransactions).HasConstraintName("FK__RentalTra__Assig__4D94879B");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalTransactions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RentalTra__Custo__4E88ABD4");

            entity.HasOne(d => d.RentalRequest).WithMany(p => p.RentalTransactions).HasConstraintName("FK__RentalTra__Renta__4F7CD00D");
        });

        modelBuilder.Entity<ReturnRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReturnRe__3214EC076189C473");

            entity.Property(e => e.AdditionalCharges).HasDefaultValue(0.00m);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LateReturnFee).HasDefaultValue(0.00m);

            entity.HasOne(d => d.RentalTransaction).WithMany(p => p.ReturnRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ReturnRec__Renta__5070F446");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07394E201C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
