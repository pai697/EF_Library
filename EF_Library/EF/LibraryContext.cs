using System;
using System.Collections.Generic;
using EF_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        string? connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder
            .UseLazyLoadingProxies() // lazy loading
            .UseSqlServer(connectionString);
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
            entity.Property(e => e.SerialNumber).HasDefaultValue("undefined");
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
            entity.ToTable(t => t.HasCheckConstraint("Shelf", "Shelf > 0 AND Shelf < 100"));
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK_Reader");
            entity.ToTable("Reader");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.MiddleName).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Number).HasMaxLength(13);
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

        Location location1 = new Location { LocationId = 1, Room = 1, Shelf = 1 };
        Location location2 = new Location { LocationId = 2, Room = 2, Shelf = 3 };
        Location location3 = new Location { LocationId = 3, Room = 3, Shelf = 3 };

        Author author1 = new Author
        {
            Id = 1,
            Name = "Name1",
            MiddleName = "MiddleName1",
            Surname = "Surname1"
        };

        Author author2 = new Author
        {
            Id = 2,
            Name = "Name2",
            MiddleName = "MiddleName2",
            Surname = "Surname2"
        };

        Author author3 = new Author
        {
            Id = 3,
            Name = "Name3",
            MiddleName = "MiddleName3",
            Surname = "Surname3"
        };

        Book book1 = new Book
        {
            Id = 1,
            Name = "Book1",
            Genre = "Genre1",
            DateTaken = DateTime.Now,
            SerialNumber = "1111",
            Location = location1.LocationId,
            AuthorId = author1.Id
        };

        Book book2 = new Book
        {
            Id = 2,
            Name = "Book2",
            Genre = "Genre2",
            DateTaken = DateTime.Now.AddYears(1),
            SerialNumber = "2222",
            Location = location2.LocationId,
            AuthorId = author2.Id
        };

        Book book3 = new Book
        {
            Id = 3,
            Name = "Book3",
            Genre = "Genre3",
            DateTaken = DateTime.Now.AddDays(1),
            SerialNumber = "3333",
            Location = location3.LocationId,
            AuthorId = author3.Id
        };

        Worker worker1 = new Worker
        {
            WorkerId = 1,
            Position = "position1",
            Name = "name1",
            Surname = "surname1",
        };

        Worker worker2 = new Worker
        {
            WorkerId = 2,
            Position = "position2",
            Name = "name2",
            Surname = "surname2",
        };

        Worker worker3 = new Worker
        {
            WorkerId = 3,
            Position = "position3",
            Name = "name3",
            Surname = "surname3",
        };

        Reader reader1 = new Reader
        {
            ReaderId = 1,
            Name = "name1",
            MiddleName = "middle1",
            Surname = "surname1",
            Email = "1@email.com",
            Number = "380671234201"
        };
        Reader reader2 = new Reader
        {
            ReaderId = 2,
            Name = "name2",
            MiddleName = "middle2",
            Surname = "surname2",
            Email = "2@email.com",
            Number = "380671234202"
        };

        Reader reader3 = new Reader
        {
            ReaderId = 3,
            Name = "name3",
            MiddleName = "middle3",
            Surname = "surname3",
            Email = "3@email.com",
            Number = "380671234203"
        };

        ReadingRoom readingroom1 = new ReadingRoom
        {
            ReadingRoomId = 1,
            WorkerId = worker1.WorkerId,
            Room = 1
        };

        ReadingRoom readingroom2 = new ReadingRoom
        {
            ReadingRoomId = 2,
            WorkerId = worker2.WorkerId,
            Room = 2
        };

        ReadingRoom readingroom3 = new ReadingRoom
        {
            ReadingRoomId = 13,
            WorkerId = worker3.WorkerId,
            Room = 3
        };

        RentedBook rentedbook1 = new RentedBook
        {
            ReaderId = reader1.ReaderId,
            BookId = book1.Id,
            BeginDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(5),
            WorkerId = worker1.WorkerId
        };

        RentedBook rentedbook2 = new RentedBook
        {
            ReaderId = reader2.ReaderId,
            BookId = book2.Id,
            BeginDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            WorkerId = worker2.WorkerId
        };

        RentedBook rentedbook3 = new RentedBook
        {
            ReaderId = reader3.ReaderId,
            BookId = book3.Id,
            BeginDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            WorkerId = worker3.WorkerId
        };

        modelBuilder.Entity<Author>().HasData(author1, author2, author3);
        modelBuilder.Entity<Location>().HasData(location1, location2, location3);
        modelBuilder.Entity<Book>().HasData(book1, book2, book3);
        modelBuilder.Entity<Worker>().HasData(worker1, worker2, worker3);
        modelBuilder.Entity<Reader>().HasData(reader1, reader2, reader3);
        modelBuilder.Entity<ReadingRoom>().HasData(readingroom1, readingroom2, readingroom3);
        modelBuilder.Entity<RentedBook>().HasData(rentedbook1, rentedbook2, rentedbook3);
    }
}