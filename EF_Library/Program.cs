﻿using EF_Library.EF;
using EF_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;

Console.WriteLine("Read: ");
LibraryContext context = new LibraryContext();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
Read_LINQ_Query_Syntax();
Console.WriteLine("\nUpdate: ");
Update();
Read_LINQ_Query_Syntax();
Console.WriteLine("\nDelete: ");
Delete();
Read_LINQ_Query_Syntax();

void Read_LINQ_Query_Syntax()
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
    var temp = context.Books.Where(x => x.AuthorId == 3).Single();
    context.Books.Remove(temp);
    context.SaveChanges();
}