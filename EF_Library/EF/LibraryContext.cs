using System;
using System.Collections.Generic;
using EF_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF_Library.EF;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<ReadingRoom> ReadingRooms { get; set; }
    public DbSet<RentedBook> RentedBooks { get; set; }
    public DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        string connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Author");
            entity.ToTable("Author");
            entity.Property(e => e.MiddleName).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Surname).HasMaxLength(30);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Book");
            entity.ToTable("Book");
            entity.Property(e => e.DateTaken).HasColumnType("date");
            entity.Property(e => e.Genre).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(30);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Book_AuthorId");

            entity.HasOne(d => d.LocationNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.Location)
                .HasConstraintName("FK_Book_Location");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK_Location");
            entity.ToTable("Location");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK_Reader");
            entity.ToTable("Reader");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.MiddleName).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Number).HasMaxLength(20);
            entity.Property(e => e.Surname).HasMaxLength(30);
        });

        modelBuilder.Entity<ReadingRoom>(entity =>
        {
            entity.HasKey(e => e.ReadingRoomId).HasName("PK_Reading");
            entity.ToTable("ReadingRoom");
            entity.HasOne(d => d.Worker).WithMany(p => p.ReadingRooms)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("FK_ReadingRoom_Worker");
        });

        modelBuilder.Entity<RentedBook>(entity =>
        {
            entity.HasKey(e => new { e.ReaderId, e.BookId }).HasName("PK_RentedBook");
            entity.ToTable("RentedBook");
            entity.Property(e => e.BeginDate).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.HasOne(d => d.Book).WithMany(p => p.RentedBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentedBook_Book");

            entity.HasOne(d => d.Reader).WithMany(p => p.RentedBooks)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentedBook_Reader");

            entity.HasOne(d => d.Worker).WithMany(p => p.RentedBooks)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("FK_RentedBook_Worker");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("PK_Worker");
            entity.ToTable("Worker");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Position).HasMaxLength(30);
            entity.Property(e => e.Surname).HasMaxLength(30);
        });
    }
}
