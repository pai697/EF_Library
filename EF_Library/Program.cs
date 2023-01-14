using Azure.Core;
using EF_Library;
using EF_Library.EF;
using EF_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using System.Threading;

Console.WriteLine("Create database!");
CreateDatabase();
MostPopularAuthors();

// Захист 3 роботи
void MostPopularAuthors()
{
    LibraryContext context = new LibraryContext();
    var query = context.Books
        .Join(
            context.Authors,
            book => book.AuthorId,
            author => author.Id,
            (book, author) => new { Author = author.Id, Book = book.Id }
        )
        .GroupBy(p => p.Author)
        .Select(m => new { m.Key, Count = m.Count() })
        .OrderByDescending(p => p.Count)
        .Take(3);
    foreach (var it in query)
    {
        Console.WriteLine("Id: " + it.Key);
        Console.WriteLine("Count: " + it.Count);
        Console.WriteLine("-----");
    }
}

void CreateDatabase()
{
    LibraryContext context = new LibraryContext();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var createSql =
        @"
                create procedure [dbo].[GetWorkers] as
                begin
                    select * from dbo.Worker
                    order by Name desc
                end
            ";

    context.Database.ExecuteSqlRaw(createSql);

    createSql =
        @"
                create function [dbo].[SearchBook] (@id int)
                returns table
                as
                return
                    select * from dbo.Book
                    where Id = @id
            ";

    context.Database.ExecuteSqlRaw(createSql);
}
void Read()
{
    LibraryContext context = new LibraryContext();

    var query =
        from book in context.Books
        join author in context.Authors on book.AuthorId equals author.Id
        select new { Book = book, Author = author, };

    foreach (var item in query)
    {
        Console.WriteLine($"Book: {item.Book.Name} {item.Book.Author.Id}");
        Console.WriteLine($"Author: {item.Author.Name} {item.Author.Surname}");
    }
}

void Update()
{
    LibraryContext context = new LibraryContext();

    var temp = context.Authors.Where(x => x.Id == 3).First();

    temp.Surname = "Surname_Update";

    context.SaveChanges();
}

void Delete()
{
    LibraryContext context = new LibraryContext();
    var temp = context.Workers.Where(x => x.WorkerId == 3).Single();
    context.Workers.Remove(temp);
    context.SaveChanges();

    var query = from row in context.Workers select row;
    foreach (var line in query)
    {
        Console.WriteLine(line.Name + " " + line.Surname + " " + line.WorkerId);
    }
}
void Insert()
{
    LibraryContext context = new LibraryContext();
    Worker worker = new Worker
    {
        Position = "position3",
        Name = "name3",
        Surname = "surname3",
    };

    context.Workers.Add(worker);
    context.SaveChanges();

    var query = from row in context.Workers select row;
    foreach (var line in query)
    {
        Console.WriteLine(line.Name + " " + line.Surname + " " + line.WorkerId);
    }
}

void Distinct()
{
    LibraryContext context = new LibraryContext();
    var query = context.RentedBooks.Select(m => m.WorkerId).Distinct().ToList();
    foreach (var worker in query)
    {
        Console.WriteLine($"{worker}");
    }
}

void Top()
{
    LibraryContext context = new LibraryContext();
    var query = (from t in context.Workers select t).Take(2);
    foreach (var worker in query)
    {
        Console.WriteLine($"{worker.Name}");
    }
}

void OrderBy()
{
    LibraryContext context = new LibraryContext();
    var query = from t in context.Workers orderby t.Name descending select t;
    foreach (var worker in query)
    {
        Console.WriteLine($"{worker.Name}");
    }
}

void Union()
{
    LibraryContext context = new LibraryContext();
    var query = (from t in context.Readers select t.Name)
        .Union(from t in context.Workers select t.Name)
        .ToList();
    foreach (var t in query)
    {
        Console.WriteLine($"{t}");
    }
}

void Except()
{
    LibraryContext context = new LibraryContext();
    var query = (from t in context.Readers select t.Name)
        .Except(from t in context.Workers select t.Name)
        .ToList();
    foreach (var t in query)
    {
        Console.WriteLine($"{t}");
    }
}

void Intersect()
{
    LibraryContext context = new LibraryContext();
    var query = (from t in context.Readers select t.Name)
        .Intersect(from t in context.Workers select t.Name)
        .ToList();
    foreach (var t in query)
    {
        Console.WriteLine($"{t}");
    }
}

void Join()
{
    LibraryContext context = new LibraryContext();
    var query =
        from t in context.ReadingRooms
        join c in context.Workers on t.WorkerId equals c.WorkerId
        select new { t, c };
    foreach (var t in query)
    {
        Console.WriteLine($"{t.c.Name}");
    }
}

void GroupBy()
{
    LibraryContext context = new LibraryContext();
    var query =
        from t in context.Workers
        join c in context.ReadingRooms on t.WorkerId equals c.WorkerId
        group t by t.Name;
    foreach (var g in query)
    {
        Console.WriteLine($"{g.Key}");
    }
}

void Count()
{
    LibraryContext context = new LibraryContext();

    var workers = context.Workers.Count();

    Console.WriteLine($"{workers}");
}

void EagerLoading()
{
    LibraryContext context = new LibraryContext();
    // var readingRoom = context.ReadingRooms.ToList(); will get empty list
    var readingRoom = context.ReadingRooms.Include(u => u.Worker).ToList();
    readingRoom.ForEach(t => Console.WriteLine(t.Worker?.WorkerId));
}

void ExplicitLoading()
{
    LibraryContext context = new LibraryContext();
    ReadingRoom? readingRoom = context.ReadingRooms.FirstOrDefault();
    if (readingRoom != null)
    {
        context.Entry(readingRoom).Reference(p => p.Worker).Load();
        Console.WriteLine(readingRoom.Worker?.WorkerId);
    }
}

void LazyLoading()
{
    LibraryContext context = new LibraryContext();
    var readingRoom = context.ReadingRooms.ToList();
    readingRoom.ForEach(t => Console.WriteLine(t.Worker?.WorkerId));
}

void AsNoTracking()
{
    LibraryContext context = new LibraryContext();
    var worker = context.Workers.AsNoTracking().FirstOrDefault();
    if (worker != null)
    {
        worker.Name = "New name";
        context.SaveChanges();
    }

    var workers = context.Workers.AsNoTracking().ToList();
    foreach (var item in workers)
        Console.WriteLine(item.Name);
}

void Procedure()
{
    LibraryContext context = new LibraryContext();

    var workers = context.Workers.FromSqlRaw("EXECUTE dbo.GetWorkers").ToList();

    foreach (var item in workers)
    {
        Console.WriteLine($"{item.WorkerId} " + $"{item.Name}");
    }
}

void Function()
{
    LibraryContext context = new LibraryContext();

    var books = context.Books.FromSqlRaw("SELECT * FROM dbo.SearchBook(2)").ToList();

    foreach (var item in books)
    {
        Console.WriteLine($"{item.Id} " + $"{item.Name}");
    }
}

async Task AsyncAdd()
{
    LibraryContext context = new LibraryContext();
    for (int i = 0; i < 10; i++)
    {
        await context.Workers.AddAsync(
            new Worker
            {
                Name = i.ToString(),
                Surname = i.ToString(),
                Position = i.ToString()
            }
        );
        await context.SaveChangesAsync();
    }
}

async Task AsyncRead()
{
    LibraryContext context = new LibraryContext();
    var list = await context.Workers.ToListAsync();
    foreach (var it in list)
    {
        Console.WriteLine(it.Name);
    }
}

void MutexRead()
{
    using (LibraryContext context = new LibraryContext())
    {
        var list = context.Workers.ToList();
        Mutex mutex = new Mutex();
        foreach (var it in list)
        {
            Thread.Sleep(100);
            Thread newThread =
                new(() =>
                {
                    mutex.WaitOne();
                    Console.WriteLine(it.Name);
                    mutex.ReleaseMutex();
                });
            newThread.Start();
        }
    }
}

void MutexWrite()
{
    Mutex mutex = new Mutex();
    LibraryContext context = new LibraryContext();
    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(500);
        Thread myThread =
            new(() =>
            {
                mutex.WaitOne();
                context.Workers.Add(
                    new Worker
                    {
                        Name = i.ToString(),
                        Surname = i.ToString(),
                        Position = i.ToString()
                    }
                );
                context.SaveChanges();
                mutex.ReleaseMutex();
            });
        myThread.Start();
    }
}

void LockRead()
{
    using (LibraryContext context = new LibraryContext())
    {
        var list = context.Workers.ToList();
        object locker = new object();
        foreach (var it in list)
        {
            Thread.Sleep(100);
            Thread newThread =
                new(() =>
                {
                    lock (locker)
                    {
                        Console.WriteLine(it.Name);
                    }
                });
            newThread.Start();
        }
    }
}

void LockWrite()
{
    object locker = new object();
    LibraryContext context = new LibraryContext();
    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(500);
        Thread myThread =
            new(() =>
            {
                lock (locker)
                {
                    context.Workers.Add(
                        new Worker
                        {
                            Name = i.ToString(),
                            Surname = i.ToString(),
                            Position = i.ToString()
                        }
                    );
                    context.SaveChanges();
                }
            });
        myThread.Start();
    }
}
