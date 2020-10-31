using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;

namespace Task3_WebApiCore.BooksDB.Entities.DbEntity
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public ICollection<AuthorBook> Authors { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public Book() => this.Authors = new HashSet<AuthorBook>();

        public override string ToString() => this.Title + " - " + this.Year;

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Book book:
                        return this.Title.Equals(book.Title) && this.Year.Equals(book.Year);
                    case WebBook webBook:
                        return this.Title.Equals(webBook.Title) && this.Year.Equals(webBook.Year);
                    default:
                        break;
                }
            }
            return false;            
        }
    }
}
