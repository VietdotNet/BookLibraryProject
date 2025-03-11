using System;
using System.Collections.Generic;
using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject;

public partial class BookLibraryManagementProjectContext : DbContext
{
    public BookLibraryManagementProjectContext()
    {
    }

    public BookLibraryManagementProjectContext(DbContextOptions<BookLibraryManagementProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<BorrowRecord> BorrowRecords { get; set; }

    public virtual DbSet<BorrowStatus> BorrowStatuses { get; set; }

    public virtual DbSet<FineRule> FineRules { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsStatus> NewsStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserIdtracker> UserIdtrackers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database= BookLibraryManagement_Project;UID=sa;PWD=vietdeptrai123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC074F293F74");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AddedById)
                .HasMaxLength(10)
                .HasColumnName("AddedByID");
            entity.Property(e => e.Author).HasMaxLength(50);
            entity.Property(e => e.AvailableQuantity).HasComputedColumnSql("([Quantity]-[BorrowedQuantity])", true);
            entity.Property(e => e.BorrowedQuantity).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasDefaultValue("Tiếng Việt");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue(0);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AddedBy).WithMany(p => p.Books)
                .HasForeignKey(d => d.AddedById)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Books_AddedBy");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Books_Category");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookCate__3214EC07AB1C94A9");

            entity.HasIndex(e => e.Name, "UQ__BookCate__737584F6D60AB821").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_BookCategories_Parent");
        });

        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BorrowRe__3214EC07D179B89C");

            entity.ToTable(tb => tb.HasTrigger("trg_CalculateFine"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.FineAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PickupDeadline).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UserId).HasMaxLength(10);

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowRecords)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Borrow_Book");

            entity.HasOne(d => d.Staff).WithMany(p => p.BorrowRecordStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Borrow_Staff");

            entity.HasOne(d => d.Status).WithMany(p => p.BorrowRecords)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Borrow_Status");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowRecordUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Borrow_User");
        });

        modelBuilder.Entity<BorrowStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BorrowSt__3214EC072CF92F8B");

            entity.ToTable("BorrowStatus");

            entity.HasIndex(e => e.StatusName, "UQ__BorrowSt__05E7698AFB2A896A").IsUnique();

            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<FineRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FineRule__3214EC070617444E");

            entity.Property(e => e.FinePerDay).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3214EC07450A7ACE");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedByStaffId).HasMaxLength(10);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedByStaffId).HasMaxLength(10);

            entity.HasOne(d => d.CreatedByStaff).WithMany(p => p.NewsCreatedByStaffs)
                .HasForeignKey(d => d.CreatedByStaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_News_CreatedBy");

            entity.HasOne(d => d.NewsStatusNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsStatus)
                .HasConstraintName("FK_News_Status");

            entity.HasOne(d => d.UpdatedByStaff).WithMany(p => p.NewsUpdatedByStaffs)
                .HasForeignKey(d => d.UpdatedByStaffId)
                .HasConstraintName("FK_News_UpdatedBy");
        });

        modelBuilder.Entity<NewsStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NewsStat__3214EC07CBB20FDB");

            entity.ToTable("NewsStatus");

            entity.HasIndex(e => e.StatusName, "UQ__NewsStat__05E7698A5AF46D1E").IsUnique();

            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07EF957F00");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F67B15EB31").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(10);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07917583C7");

            entity.ToTable(tb => tb.HasTrigger("trg_GenerateUserID"));

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534D8DCC966").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.StudentCode).HasMaxLength(8);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Role");
        });

        modelBuilder.Entity<UserIdtracker>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__UserIDTr__8AFACE1ADCC8FFCE");

            entity.ToTable("UserIDTracker");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.LastId)
                .HasDefaultValue(0)
                .HasColumnName("LastID");
            entity.Property(e => e.Prefix).HasMaxLength(2);

            entity.HasOne(d => d.Role).WithOne(p => p.UserIdtracker)
                .HasForeignKey<UserIdtracker>(d => d.RoleId)
                .HasConstraintName("FK_UserIDTracker_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
