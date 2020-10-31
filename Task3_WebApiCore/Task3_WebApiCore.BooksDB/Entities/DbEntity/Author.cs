using System;
using System.Collections.Generic;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;

namespace Task3_WebApiCore.BooksDB.Entities.DbEntity
{
    public class Author
    {
        public int AuthorId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<AuthorBook> Books { get; set; }

        public Author() => Books = new HashSet<AuthorBook>();

        public override string ToString() => FirstName + " " + LastName;

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Author author:
                        return this.FirstName.Equals(author.FirstName) && this.LastName.Equals(author.LastName);
                    case WebAuthor webAuthor:
                        return this.FirstName.Equals(webAuthor.FirstName) && this.LastName.Equals(webAuthor.LastName);
                    default:
                        break;
                }
            }
            return false;
        }
    }
}