using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_EFCore.BooksDB.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<AuthorBook> Books { get; set; }

        public Author() => Books = new HashSet<AuthorBook>();

        public override string ToString() => FirstName + " " + LastName;
    }
}
