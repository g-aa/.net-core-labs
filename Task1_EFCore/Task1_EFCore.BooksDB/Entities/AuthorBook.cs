using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_EFCore.BooksDB.Entities
{
    public class AuthorBook
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
