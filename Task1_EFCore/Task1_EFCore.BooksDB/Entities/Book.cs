using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_EFCore.BooksDB.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int Year { get; set; }

        public ICollection<AuthorBook> Authors { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public Book() => this.Authors = new HashSet<AuthorBook>();

        public override string ToString() => this.BookTitle + " - " + this.Year;
    }
}
