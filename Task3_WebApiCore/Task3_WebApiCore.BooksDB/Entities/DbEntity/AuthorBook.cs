using System;
using System.Collections.Generic;
using System.Text;

namespace Task3_WebApiCore.BooksDB.Entities.DbEntity
{
    public class AuthorBook
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
