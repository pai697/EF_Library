using EF_Library.EF;
using EF_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;

Console.WriteLine("Create database!");
CreateDatabase();
//Console.WriteLine("Read: ");
//Read();
//Console.WriteLine("\nUpdate: ");
//Update();
//Read();
//Console.WriteLine("\nDelete: ");
//Delete();
//Console.WriteLine("\nInsert:");
//Insert();
//Console.WriteLine("\nDistinct:");
//Distinct();
//Console.WriteLine("\nTop:");
//Top();
//Console.WriteLine("\nOrder by:");
//OrderBy();
//Console.WriteLine("\nUnion:");
//Union();
//Console.WriteLine("\nExcept:");
//Except();
//Console.WriteLine("\nIntersect:");
//Intersect();
//Console.WriteLine("\nJoin:");
//Join();
//Console.WriteLine("\nGroup by:");
//GroupBy();
//Console.WriteLine("\nCount:");
//Count();
Console.WriteLine("\nEager loading");
EagerLoading();
Console.WriteLine("\nLazy loading");
LazyLoading();
Console.WriteLine("\nExplicit loading");
ExplicitLoading();

void CreateDatabase()
{
    LibraryContext context = new LibraryContext();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}
void Read()
{
    LibraryContext context = new LibraryContext();

    var query = from book in context.Books
                join author in context.Authors
                on book.AuthorId equals author.Id
                select new
                {
                    Book = book,
                    Author = author,
                };

    foreach (var item in query)
    {
        Console.WriteLine("----------------------------------------------------------");
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
    var query = (from t in context.Readers select t.Name).
        Union(from t in context.Workers select t.Name).ToList();
    foreach (var t in query)
    {
        Console.WriteLine($"{t}");
    }
}

void Except()
{
    LibraryContext context = new LibraryContext();
    var query = (from t in context.Readers select t.Name).
        Except(from t in context.Workers select t.Name).ToList();
    foreach (var t in query)
    {
        Console.WriteLine($"{t}");
    }
}

void Intersect()
{
    LibraryContext context = new LibraryContext();
    var query = (from t in context.Readers select t.Name).
        Intersect(from t in context.Workers select t.Name).ToList();
    foreach (var t in query)
    {
        Console.WriteLine($"{t}");
    }
}

void Join()
{
    LibraryContext context = new LibraryContext();
    var query = from t in context.ReadingRooms
                join c in context.Workers
                on t.WorkerId equals c.WorkerId
                select new { t, c };
    foreach (var t in query)
    {
        Console.WriteLine($"{t.c.Name}");
    }
}

void GroupBy()
{
    LibraryContext context = new LibraryContext();
    var query = from t in context.Workers
                join c in context.ReadingRooms
                on t.WorkerId equals c.WorkerId
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
    //var readingRoom = context.ReadingRooms.ToList(); will get empty list
    var readingRoom = context.ReadingRooms.Include(u => u.Worker).ToList();
    readingRoom.ForEach(t => Console.WriteLine(t.Worker?.WorkerId));
}

void ExplicitLoading()
{
    LibraryContext context = new LibraryContext();
    ReadingRoom? readingRoom = context.ReadingRooms.FirstOrDefault();
    if(readingRoom != null)
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