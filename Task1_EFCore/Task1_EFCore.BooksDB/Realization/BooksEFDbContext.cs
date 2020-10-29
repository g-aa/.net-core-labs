using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Task1_EFCore.BooksDB.Entities;

namespace Task1_EFCore.BooksDB.Realization
{
    internal class BooksEFDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public BooksEFDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyBooksDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // настройка ключа в AuthorBook:
            modelBuilder.Entity<AuthorBook>().HasKey((AuthorBook ab) => new { ab.AuthorId, ab.BookId });

            // настройка связи AuthorBook - Author:
            modelBuilder.Entity<AuthorBook>().HasOne((AuthorBook ab) => ab.Author).WithMany((Author a) => a.Books).HasForeignKey((AuthorBook ab) => ab.AuthorId);

            // настройка связи AuthorBook - Book:
            modelBuilder.Entity<AuthorBook>().HasOne((AuthorBook ab) => ab.Book).WithMany((Book b) => b.Authors).HasForeignKey((AuthorBook ab) => ab.BookId);
        }
    }
}
