using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_EFCore.BooksDB.Entities
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Title { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<Book> Books { get; set; }

        public Publisher()
        {
            this.Addresses = new HashSet<Address>();
            this.Books = new HashSet<Book>();
        }

        public override string ToString() => this.Title;
    }
}
